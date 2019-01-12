using System;
using System.Security.Cryptography;
using System.IO;

namespace ServerSystem.Security
{
    public class MachineKeyGenerator
{

        /// <summary>
        /// Número de días que contiene la firma que se genera
        /// </summary>
        /// <value></value>
        public uint VigenciaDeFirmaEnDias { get; private set; }

        /// <summary>
        /// Vector de inicialización para el firmado
        /// </summary>
        private byte [] vector;

        /// <summary>
        /// Llave de inicializaron para el firmado 
        /// </summary>
        private byte [] llave;

        public MachineKeyGenerator(uint vigenciaDeFirmaEnDias, byte[] vector, byte[] llave)
        {   
            if(vigenciaDeFirmaEnDias==0) throw new ArgumentException("La vigencia en días debe ser mayot a 0");
            ValidaLlave(llave);
            ValidaVector(vector);

            this.VigenciaDeFirmaEnDias = vigenciaDeFirmaEnDias;
            this.vector = vector;
            this.llave = llave;

        }

        internal MachineSignature GeneraFirmaDeMachineSignature(MachineInfo machineInfo) {
            
            Guid id = Guid.NewGuid();
            DateTime fechaDeFirma = DateTime.Now;
            DateTime fechaDeExpiracion = fechaDeFirma.AddDays(this.VigenciaDeFirmaEnDias);
            String firma = GeneraFirma(machineInfo,id,fechaDeExpiracion);
            
            return new MachineSignature(
                id:id,
                machineInfo : machineInfo,
                signDate:fechaDeFirma,
                expirationDate:fechaDeExpiracion
            );
        }

        static void ValidaLlave(byte[] arr){
            var length = arr.Length;
            Console.WriteLine(length);
            var validLength = new int[3]{16,24,32};
            
            if( length != validLength[0] && 
                length != validLength[1] && 
                length != validLength[2] ) throw new ArgumentException("La llave es inválida");
        }
        static void ValidaVector(byte[] arr){
            if(arr.Length != 16) throw new ArgumentException("Vector es inválido");
        }

        public String Decriptar(String cadenaCifrada){
            string textoPlano = null;
            using(AesManaged aes = new AesManaged()) {
                    
                    ICryptoTransform decryptor = aes.CreateDecryptor(this.llave, this.vector); 
                    
                    using(MemoryStream ms = new MemoryStream(Convert.FromBase64String(cadenaCifrada))) {
                    
                        using(CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    
                            using(StreamReader reader = new StreamReader(cs))
                            textoPlano = reader.ReadToEnd();
                        }
                    }
                }
            return textoPlano; 
        }

        private string GeneraFirma(MachineInfo info, Guid id, DateTime fechaDeExpiracion){
            
            byte[] firma ;
            using(var aes = Aes.Create()){
                aes.Key = this.llave;
                aes.IV = this.vector;
                 using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var resultStream = new MemoryStream())
                    {
                        using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                        using (var plainStream = new MemoryStream(CadenaAFirmar(info,fechaDeExpiracion)))
                        {
                            plainStream.CopyTo(aesStream);
                        }

                        firma = resultStream.ToArray();
                    }
            }

            return Convert.ToBase64String(firma);
       }

       private byte[] CadenaAFirmar(MachineInfo info, DateTime fechaDeExpiracion)
       {
           var texto = info+"|"+fechaDeExpiracion.ToString("dd/MM/yyyy");
           Console.WriteLine(texto);
           return System.Text.UTF8Encoding.UTF8.GetBytes(texto);
       }


    }
}


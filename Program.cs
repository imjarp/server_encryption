using System;

namespace ServerSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ServerSystem.Security.MachineInfo m = new Security.MachineInfo("Some Text");
            var llave = GetRandomData(128);
            var vector = GetRandomData(128);

            ServerSystem.Security.MachineKeyGenerator keyGenerator = 
                            new Security.MachineKeyGenerator(30,vector,llave);

            var result = keyGenerator.GeneraFirmaDeMachineSignature(m);
            Console.WriteLine(result.Firma);
            var decriptado = keyGenerator.Decriptar(result.Firma);
            
            Console.WriteLine(decriptado);
        }

        private static byte[] GetRandomData(int bits)
        {
            var result = new byte[bits / 8];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }
    }
}

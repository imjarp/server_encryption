using System;

namespace ServerSystem.Security{

    internal class MachineSignature
    {
        public Guid id { get; }

        public MachineInfo MachineInfo { get; private set; }

        public DateTime SignDate { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        internal MachineSignature(Guid id, MachineInfo machineInfo, DateTime signDate, DateTime expirationDate){
            this.id = id;
            this.MachineInfo = machineInfo;
            this.SignDate = signDate;
            this.ExpirationDate = expirationDate;
        }

        public override String ToString(){
            return String.Format("{0}|{1}|{2}|{3}",this.id,
                                                   this.SignDate.ToString("dd/MM/yyyy"),
                                                   this.ExpirationDate.ToString("dd/MM/yyyy"),
                                                   this.MachineInfo);
        }


        public static MachineSignature  CrearDesdeCadena(string cadena){
                var args = cadena.Split('|');
                return new MachineSignature(
                    id: Guid.Parse(args[0]) ,
                    signDate:DateTime.Parse(args[1]) ,
                    expirationDate:DateTime.Parse(args[2]),
                    machineInfo: MachineInfo.DesdeCadena(args[3]) 
                );
        }

    }
}
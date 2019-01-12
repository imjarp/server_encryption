
using System;

namespace ServerSystem.Security
{
    internal class MachineInfo
    {

        private string _macAddress;
        public MachineInfo(string macAddress){
            if(string.IsNullOrWhiteSpace(macAddress)){
                throw new ArgumentNullException("macAddress");
            }

            this._macAddress = macAddress;
        }

        public override String ToString(){
            return this._macAddress;
        }

        public static MachineInfo DesdeCadena(String cadena){
            return new MachineInfo(cadena);
        }

    }

}
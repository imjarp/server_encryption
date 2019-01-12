using System;

namespace ServerSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            ServerSystem.Security.MachineInfo m = 
                    new Security.MachineInfo("Here we need the mac address or may some other data");

            // The next needs to be hard coded
            var llave = GetRandomData(128);
            var vector = GetRandomData(128);
            var validityDays = 30u;

            ServerSystem.Security.MachineKeyGenerator keyGenerator = 
                            new Security.MachineKeyGenerator(validityDays,vector,llave);

            var result = keyGenerator.GeneraFirmaDeMachineSignature(m);
            
            var machineInfo = keyGenerator.Decriptar(result);
            
            Console.WriteLine(machineInfo);
        }

        private static byte[] GetRandomData(int bits)
        {
            var result = new byte[bits / 8];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }
    }
}

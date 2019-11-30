using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Debug
{
    public static class ReplicatedAlgorithm
    {
        public static string GenerateKey()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            if (networkInterface == null)
            {
                throw new InvalidOperationException($"Cannot access '{nameof(NetworkInterface)}'.");
            }

            var dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            var key = networkInterface
                .GetPhysicalAddress()
                .GetAddressBytes()
                .Select((@byte, index) => @byte ^ dateBytes[index])
                .Select(
                    @byte => @byte <= 999
                        ? @byte * 10
                        : @byte)
                .ToArray();

            return string.Join("-", key);
        }

        public static bool Check(string[] value)
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            if (networkInterface == null)
            {
                return false;
            }

            var dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            var key = networkInterface
                .GetPhysicalAddress()
                .GetAddressBytes()
                .Select((@byte, index) => @byte ^ dateBytes[index])
                .Select(
                    @byte => @byte <= 999
                        ? @byte * 10
                        : @byte)
                .ToArray();

            var input = value.Select(int.Parse).ToArray();
            return key
                .Select((@byte, index) => @byte - input[index])
                .All(res => res == 0);
        }
    }
}

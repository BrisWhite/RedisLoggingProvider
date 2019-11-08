using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    internal class Utils
    {
        public static string GetIpAddress()
        {
            var allIntefaces = NetworkInterface.GetAllNetworkInterfaces();

            var firstUpInterface = allIntefaces
                .OrderByDescending(c => c.Speed)
                .FirstOrDefault(c =>
                c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);
            if (firstUpInterface != null)
            {
                var props = firstUpInterface.GetIPProperties();
                // get first IPV4 address assigned to this interface
                var firstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault();

                return firstIpV4Address.ToString();
            }
            return "::";
        }
    }
}

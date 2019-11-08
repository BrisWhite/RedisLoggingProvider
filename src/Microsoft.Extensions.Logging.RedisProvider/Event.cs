using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class Event
    {
        /// <summary>
        /// 主要记录调用来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        public string HostName { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结构化信息
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [JsonIgnore]
        public Exception Exception { get; set; }

        public Event()
        {
            Data = new Dictionary<string, object>();
            TimeStamp = DateTime.Now;
            Host = GetIpAddress();
            HostName = Dns.GetHostName();
        }

        private string GetIpAddress()
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

    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}

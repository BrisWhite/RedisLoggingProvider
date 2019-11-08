using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public abstract class BaseEventData : IEventData
    {
        /// <summary>
        /// 请求的唯一ID
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// 主要记录调用来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
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

        public BaseEventData()
        {
            Data = new Dictionary<string, object>();
            TimeStamp = DateTime.Now;
            Host = Utils.GetIpAddress();
            HostName = Dns.GetHostName();
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

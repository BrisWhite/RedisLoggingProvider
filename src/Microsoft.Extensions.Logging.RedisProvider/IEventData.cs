using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public interface IEventData
    {
        /// <summary>
        /// 请求的唯一ID
        /// </summary>
        string ActionId { get; set; }

        string Source { get; set; }

        string Level { get; set; }

        DateTime TimeStamp { get; set; }

        string Message { get; set; }

        Dictionary<string, object> Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public static class RedisLoggingDefault
    {
        /// <summary>
        /// 操作 ID
        /// </summary>
        public static string ActionId = "ActionId";

        /// <summary>
        /// 事件 ID
        /// </summary>
        public static string EventId = "EventId";

        /// <summary>
        /// 数据集合的名称
        /// </summary>
        public static string DataPropertyName = "Data";

        /// <summary>
        /// 异常名称
        /// </summary>
        public static string ExceptionPropertyName = "exception";
    }
}

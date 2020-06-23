using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class RedisLogScopeInfo
    {
        public string ActionId { get; set; }
        public string UserId { get; set; }
    }
}

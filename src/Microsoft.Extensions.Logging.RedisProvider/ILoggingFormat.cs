using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public interface ILoggingFormat
    {
        string GetFormatLog(EventBuilder builder);
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class RedisLoggingClient
    {
        private readonly RedisPushClient _redisPushClient;
        private readonly ILoggingFormat _loggingFormat;

        public RedisLoggingClient(RedisLoggingConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Configuration = configuration;
            _redisPushClient = new Lazy<RedisPushClient>(() =>
            {
                return new RedisPushClient(configuration);
            }).Value;

            _loggingFormat = (ILoggingFormat)Activator.CreateInstance(configuration.LoggingFormatType, configuration.EventTypeProperties);
        }

        public RedisLoggingConfiguration Configuration { get; set; }

        private BaseEventData GetEventData()
        {
            return (BaseEventData)Activator.CreateInstance(Configuration.EventType);
        }

        public EventBuilder CreateEvent()
        {
            return new EventBuilder(GetEventData());
        }

        public EventBuilder CreateLog(string source, string message, LogLevel level)
        {
            return CreateEvent().SetSource(source).SetMessage(message).SetLevel(level.ToString());
        }

        public void Submit(EventBuilder builder)
        {
            var json = _loggingFormat.GetFormatLog(builder);
            _redisPushClient.Push(json);
        }
    }
}

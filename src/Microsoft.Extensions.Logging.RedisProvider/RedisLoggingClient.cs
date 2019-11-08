using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class RedisLoggingClient
    {
        private readonly Lazy<RedisPushClient> _redisPushClient;

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
            });
        }

        public RedisLoggingConfiguration Configuration { get; set; }

        public EventBuilder CreateEvent()
        {
            return new EventBuilder(new Event());
        }

        public EventBuilder CreateLog(string source, string message, LogLevel level)
        {
            return CreateEvent().SetSource(source).SetMessage(message).SetLevel(level.ToString());
        }

        private void AddJObject(JObject jObject, string name, object obj)
        {
            if (obj == null)
            {
                jObject.Add(name, new JValue("null"));
                return;
            }

            var fullName = obj.GetType().FullName;
            if (obj.GetType().IsValueType || fullName.StartsWith("System"))
            {
                if (fullName.StartsWith("System.Reflection"))
                {
                    return;
                }

                jObject.Add(name, new JValue(obj));
                return;
            }

            var keyObject = JObject.FromObject(obj);
            jObject.Add(name, keyObject);
        }

        public void Submit(EventBuilder builder)
        {
            var target = JObject.FromObject(builder.Target);
            target.Add(RedisLoggingDefault.DataPropertyName, JObject.FromObject(new object()));

            var tempObject = JObject.FromObject(new object());
            foreach (var item in builder.Target.Data)
            {
                AddJObject(tempObject, item.Key, item.Value);
            }

            if (builder.Target.Exception != null)
            {
                tempObject.Add(RedisLoggingDefault.ExceptionPropertyName, JObject.FromObject(builder.Target.Exception));
            }

            target[RedisLoggingDefault.DataPropertyName] = tempObject;
            string json = target.ToString();
            _redisPushClient.Value.Push(json);
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class LoggingFormat : ILoggingFormat
    {
        private readonly IEnumerable<string> _eventProperties;

        public LoggingFormat(IEnumerable<string> properties)
        {
            _eventProperties = properties;
        }

        private void AddJObject(JObject jObject, string name, object obj)
        {
            try
            {
                if (jObject.ContainsKey(name))
                {
                    jObject.Remove(name);
                }

                if (obj == null)
                {
                    jObject.Add(name, new JValue("null"));
                    return;
                }

                var fullName = obj.GetType().FullName;
                if (obj.GetType().IsValueType || fullName.StartsWith("System"))
                {
                    if (fullName.StartsWith("System."))
                    {
                        return;
                    }

                    jObject.Add(name, new JValue(obj));
                    return;
                }

                var keyObject = JObject.FromObject(obj);
                jObject.Add(name, keyObject);
            }
            catch (Exception)
            {
                // 不能因为日志系统出错，影响业务
            }
        }

        public string GetFormatLog(EventBuilder builder)
        {
            // 如果结构化字段里有日志事件类型中的属性，则记录下来，否则放在Data里
            var target = JObject.FromObject(builder.Target);

            var tempDictionary = new Dictionary<string, object>();

            foreach (var item in builder.Target.Data)
            {
                if (_eventProperties.Any(x => x == item.Key))
                {
                    AddJObject(target, item.Key, item.Value);
                    continue;
                }

                tempDictionary.Add(item.Key, item.Value);
            }

            if (tempDictionary.Count > 0) 
            {
                var tempObject = JObject.FromObject(new object());
                foreach (var item in tempDictionary)
                {
                    AddJObject(tempObject, item.Key, item.Value);
                }

                if (tempObject.Count > 0) 
                {
                    target.Add(RedisLoggingDefault.DataPropertyName, JObject.FromObject(new object()));
                    target[RedisLoggingDefault.DataPropertyName] = tempObject;
                }
            }

            if (builder.Target.Exception != null)
            {
                target.Add(RedisLoggingDefault.ExceptionPropertyName, JsonConvert.SerializeObject(builder.Target.Exception));
            }
            
            return target.ToString();
        }
    }
}

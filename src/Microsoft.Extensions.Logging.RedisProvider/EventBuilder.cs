using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class EventBuilder
    {
        public EventBuilder(Event ev)
        {
            Target = ev;
        }

        public Event Target { get; private set; }

        public EventBuilder SetMessage(string message)
        {
            Target.Message = message;
            return this;
        }

        public EventBuilder SetSource(string source)
        {
            Target.Source = source;
            return this;
        }

        public EventBuilder SetLevel(string level)
        {
            Target.Level = level;
            return this;
        }

        public EventBuilder AddProperty(string key, object obj)
        {
            if (Target.Data.ContainsKey(key))
            {
                Target.Data[key] = obj;
                return this;
            }

            Target.Data.Add(key, obj);
            return this;
        }

        public EventBuilder SetException(Exception exception) 
        {
            Target.Exception = exception;
            return this;
        }
    }
}

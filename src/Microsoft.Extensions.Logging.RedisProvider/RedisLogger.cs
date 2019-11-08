using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class RedisLogger : ILogger
    {
        private readonly RedisLoggingClient _client;
        private readonly string _source;

        public RedisLogger(RedisLoggingClient client, string source)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _source = source;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            string id = state.ToString();
            var scope = new LoggingScope(id);

            // Add to stack to support nesting within execution context
            LoggingScope.Push(scope);
            return scope;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None || !_client.Configuration.IsValid)
            {
                return false;
            }

            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(logLevel) || !_client.Configuration.IsValid)
                return;

            string message = formatter(state, exception);
            if (String.IsNullOrEmpty(message) && exception == null)
                return;

            var builder = _client.CreateLog(_source, message, logLevel);

            if (exception != null) 
            {
                builder.SetException(exception);
            }

            if (eventId.Id != 0)
            {
                builder.AddProperty(RedisLoggingDefault.EventId, eventId.Id);
            }

            if (LoggingScope.Current != null)
            {
                builder.AddProperty(RedisLoggingDefault.ActionId, LoggingScope.Current.Id);
            }
            if (state is IEnumerable<KeyValuePair<string, object>> stateProps)
            {
                foreach (var prop in stateProps)
                {
                    // Logging the message template is superfluous
                    if (prop.Key != "{OriginalFormat}")
                    {
                        builder.AddProperty(prop.Key, prop.Value);
                    }
                }
            }
            else
            {
                // Otherwise, attach the entire object, using its type as the name
                builder.AddProperty(state.GetType().Name, state);
            }

            _client.Submit(builder);
        }
    }
}

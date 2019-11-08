using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    internal class LoggingScope : IDisposable
    {
        private static readonly AsyncLocal<LoggingScope> _current = new AsyncLocal<LoggingScope>();

        public static LoggingScope Current
        {
            get => _current.Value;
            private set => _current.Value = value;
        }

        public static void Push(LoggingScope scope)
        {
            var temp = Current;
            Current = scope;
            Current.Parent = temp;
        }

        public string Id { get; }

        public LoggingScope Parent { get; private set; }

        public LoggingScope(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) 
            {
                id = Guid.NewGuid().ToString();
            }

            Id = id;
        }

        public void Dispose()
        {
            Current = Current.Parent;
        }
    }
}

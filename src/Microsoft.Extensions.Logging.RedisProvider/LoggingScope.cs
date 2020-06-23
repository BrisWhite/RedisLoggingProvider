using Newtonsoft.Json;
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
        public string UserId { get; }

        public LoggingScope Parent { get; private set; }

        public LoggingScope(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = Guid.NewGuid().ToString();
            }

            Id = id;
        }

        public LoggingScope(string id, string userId) : this(id)
        {
            UserId = userId;
        }

        public void Dispose()
        {
            Current = Current.Parent;
        }

        private LoggingScope GetRootParent(LoggingScope loggingScope)
        {
            return loggingScope.Parent == null ? (loggingScope) : GetRootParent(loggingScope.Parent);
        }

        private LoggingScope GetRootUserId(LoggingScope loggingScope)
        {
            if (loggingScope.Parent == null) 
            {
                return this;
            }

            return !string.IsNullOrEmpty(loggingScope.UserId) ? (loggingScope) : GetRootUserId(loggingScope.Parent);
        }

        public string GetRootId()
        {
            return GetRootParent(this).Id;
        }

        public string GetRootUserId()
        {
            return GetRootUserId(this).UserId;
        }
    }
}

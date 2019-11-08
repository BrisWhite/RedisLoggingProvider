using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class LoggingProvider : ILoggerProvider
    {
        private readonly RedisLoggingClient _client;

        public LoggingProvider(RedisLoggingClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

        public LoggingProvider(IConfiguration configuration) 
        {
            var connectionString = configuration[nameof(RedisLoggingConfiguration.ConnectionString)];
            var database = configuration.GetValue<int>(nameof(RedisLoggingConfiguration.Database));
            var projectName = configuration[nameof(RedisLoggingConfiguration.ProjectName)];

            var config = new RedisLoggingConfiguration();
            config.SetConfiguration(connectionString, database, projectName);
            _client = new RedisLoggingClient(config);
        }

        public LoggingProvider(Action<RedisLoggingConfiguration> configure) 
        {
            var config = new RedisLoggingConfiguration();
            configure(config);
            _client = new RedisLoggingClient(config);
        }

        public ILogger CreateLogger(string categoryName) 
        {
            return new RedisLogger(_client, categoryName);
        }

        public void Dispose() { }
    }
}

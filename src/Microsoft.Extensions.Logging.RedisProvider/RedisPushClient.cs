using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class RedisPushClient
    {
        private readonly RedisLoggingConfiguration _configuration;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisPushClient(RedisLoggingConfiguration configuration)
        {
            _configuration = configuration;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
            _database = _connectionMultiplexer.Value.GetDatabase(_configuration.Database);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_configuration.ConnectionString);
        }

        public void Push(string message)
        {
            _database.ListLeftPushAsync(_configuration.ProjectName, message).ConfigureAwait(false);
        }
    }
}

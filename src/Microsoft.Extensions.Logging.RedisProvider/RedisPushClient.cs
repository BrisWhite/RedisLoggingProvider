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

        public RedisPushClient(RedisLoggingConfiguration configuration)
        {
            _configuration = configuration;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        private IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_configuration.Database);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_configuration.ConnectionString);
        }

        public void Push(string message)
        {
            var database = GetDatabase();
            database.ListLeftPush(_configuration.ProjectName, message);
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public static class RedisProviderExtensions
    {
        public static ILoggingBuilder AddRedis(this ILoggingBuilder builder, IConfiguration configuration) 
        {
            builder.AddProvider(new LoggingProvider(configuration));
            return builder;
        }

        public static ILoggerFactory AddRedis(this ILoggerFactory factory, Action<RedisLoggingConfiguration> configure) 
        {
            factory.AddProvider(new LoggingProvider(configure));
            return factory;
        }
    }
}

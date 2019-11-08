using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.RedisProvider;

namespace RedisLoggingProvider.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            factory.AddRedis(x =>
            {
                x.ConnectionString = Configuration.GetSection("RedisLogging")[nameof(RedisLoggingConfiguration.ConnectionString)];
                x.Database = Configuration.GetSection("RedisLogging").GetValue<int>(nameof(RedisLoggingConfiguration.Database));
                x.ProjectName = Configuration.GetSection("RedisLogging")[nameof(RedisLoggingConfiguration.ProjectName)];
                x.EventType = typeof(CustomeLoggingEvent);
            });
        }
    }
}

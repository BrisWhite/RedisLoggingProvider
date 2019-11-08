# RedisLoggingProvider
A simple netcore extension logging tool for redis,Out of the box!

# Features
1. Based on Microsoft.Extensions.Logging,extend from ILogger and ILoggerProvider;
2. Structured Logging

# Usages
We provide two ways to use.
1. Extend from ILoggerFactory
```c#
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
      });
}
```

2. Extend from ILoggingBuilder
```c#
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddRedis(hostingContext.Configuration.GetSection("RedisLogging"));
    }).UseStartup<Startup>();
```

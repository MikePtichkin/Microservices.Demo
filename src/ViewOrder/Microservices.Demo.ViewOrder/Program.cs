using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microservices.Demo.ViewOrder;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddEnvironmentVariables("DEMO_");
    })
    .Build()
    .RunAsync();
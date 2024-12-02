using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microservices.Demo.ClientOrders;
using Microservices.Demo.ClientOrders.Extensions;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build()
    .RunOrMigrateAsync(args);

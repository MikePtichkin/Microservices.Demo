using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microservices.Demo.ClientBalance;
using Microservices.Demo.ClientBalance.Extensions;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build()
    .RunOrMigrateAsync(args);



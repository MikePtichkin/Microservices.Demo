using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microservices.Demo.ReportService;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build()
    .RunAsync();
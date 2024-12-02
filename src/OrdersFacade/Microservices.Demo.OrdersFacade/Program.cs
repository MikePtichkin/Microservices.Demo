using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microservices.Demo.OrdersFacade;
using Serilog;
using System;

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

try
{
    Log.Information("Initializing application...");

    await Host
        .CreateDefaultBuilder(args)
        .UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ServiceName", context.HostingEnvironment.ApplicationName);
        })
        .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
        .Build()
        .RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exeption");
}
finally
{
    await Log.CloseAndFlushAsync();
}

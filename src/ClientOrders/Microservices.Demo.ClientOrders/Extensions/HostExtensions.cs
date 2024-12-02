using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Extensions;

public static class HostExtensions
{
    public static async Task RunOrMigrateAsync(
        this IHost host,
        string[] args)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        if (args.Length > 0)
        {
            switch (args[0].ToLower())
            {
                case "migrate":
                    await Task.Run(runner.MigrateUp);
                    break;
                case "rollback":
                    if (args.Length > 1 && int.TryParse(args[1], out int version))
                    {
                        await Task.Run(() => runner.MigrateDown(version));
                    }
                    else
                    {
                        throw new ArgumentException("No version specified for rollback or invalid version number.");
                    }
                    break;
                default:
                    await host.RunAsync();
                    break;
            }
        }
        else
        {
            await host.RunAsync();
        }
    }
}

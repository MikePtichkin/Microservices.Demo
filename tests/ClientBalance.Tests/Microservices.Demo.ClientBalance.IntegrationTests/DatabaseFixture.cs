using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using Microservices.Demo.ClientBalance.Infra.Options;
using System.IO;

namespace Microservices.Demo.ClientBalance.IntegrationTests;

public class DatabaseFixture
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DatabaseFixture()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        serviceCollection.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        serviceCollection.AddLogging(builder => builder.AddConsole());
        serviceCollection.AddTransient<IDbConnectionFactory<NpgsqlConnection>, PostgresConnectionFactory>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Operations;
using Microservices.Demo.ClientBalance.Infra.Options;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Common;

internal sealed class PostgresConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly string _connectionString;
    private readonly ILogger<PostgresConnectionFactory> _logger;

    private NpgsqlConnection? _connection;

    public PostgresConnectionFactory(
        IOptions<ConnectionStrings> connectionStringsOptions,
        ILogger<PostgresConnectionFactory> logger)
    {
        _connectionString = connectionStringsOptions.Value.ClientBalanceServiceDb;
        _logger = logger;

        _logger.LogInformation("Initializing PostgresConnectionFactory with connection string: {ConnectionString}",
            _connectionString);
    }

    public Task<NpgsqlConnection> GetConnection(CancellationToken token)
    {
        if (_connection is not null && _connection.State is ConnectionState.Open)
        {
            return Task.FromResult(_connection);
        }
        
        return CreateAndOpenConnectionAsync(token);
    }

    private Task<NpgsqlConnection> CreateAndOpenConnectionAsync(
        CancellationToken token) => Task.Run(async () =>
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        dataSourceBuilder.MapEnum<OperationStatus>("operation_status");
        dataSourceBuilder.MapEnum<OperationType>("operation_type");

        var dataSource = dataSourceBuilder.Build();
        _connection = dataSource.CreateConnection();

        await _connection.OpenAsync(token);
        return _connection;
    }, token);

    public void Dispose()
    {
        _connection?.Dispose();
    }
}

using Microsoft.Extensions.Logging;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Abstractions;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Infra.Repositories;

public abstract class BaseRepository : IBaseRepository
{
    protected readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;
    protected readonly ILogger<BaseRepository> _logger;

    protected BaseRepository(
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        ILogger<BaseRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<ITransactionScope> CreateTransactionScope(CancellationToken cancellationToken)
    {
        var connection = await _connectionFactory.GetConnection(cancellationToken);

        var transaction = await connection.BeginTransactionAsync(cancellationToken);

        return new TransactionScope(connection, transaction, _logger);
    }
}

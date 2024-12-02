using Microsoft.Extensions.Logging;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Common;

public class TransactionScope : ITransactionScope
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction _transaction;
    private readonly ILogger _logger;
    private bool _committed = false;

    public TransactionScope(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        ILogger logger)
    {
        _connection = connection;
        _transaction = transaction;
        _logger = logger;
    }

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        await _transaction.CommitAsync(cancellationToken);
        _committed = true;
        _logger.LogInformation("Transaction committed successfully.");
    }

    public async ValueTask DisposeAsync()
    {
        if (!_committed)
        {
            try
            {
                await _transaction.RollbackAsync();
                _logger.LogInformation("Transaction rolled back successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during transaction rollback.");
            }
        }

        await _transaction.DisposeAsync();
        await _connection.DisposeAsync();
        _logger.LogInformation("Database connection closed.");
    }
}

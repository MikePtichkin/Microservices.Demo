using Microsoft.Extensions.Logging;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Operations;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Infra.Repositories;

internal sealed class OperationsRepository : BaseRepository, IOperationsRepository
{
    public OperationsRepository(
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        ILogger<BaseRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public async Task CreateTopUp(
        Operation operation,
        CancellationToken cancellationToken)
    {
        const string sql = $"""
            INSERT INTO top_ups (id, user_id, amount, type, status, created_at)
            VALUES (
                @id,
                @user_id,
                @amount,
                @type::operation_type,
                @status::operation_status,
                @created_at);
            """;

        NpgsqlParameter[] parameters =
        [
            new("id", operation.Id),
            new("user_id", operation.UserId),
            new("amount", operation.Amount),
            new("type", operation.Type),
            new("status", operation.Status),
            new("created_at", operation.CreatedAt.ToUniversalTime())
        ];

        using var connection = await _connectionFactory.GetConnection(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task CreateWithdraw(
        Operation operation,
        CancellationToken cancellationToken)
    {
        const string sql = $"""
            INSERT INTO withdraws (id, user_id, amount, type, status, created_at)
            VALUES (
                @id,
                @user_id,
                @amount,
                @type::operation_type,
                @status::operation_status,
                @created_at);
            """;

        NpgsqlParameter[] parameters =
        [
            new("id", operation.Id),
            new("user_id", operation.UserId),
            new("amount", operation.Amount),
            new("type", operation.Type),
            new("status", operation.Status),
            new("created_at", operation.CreatedAt.ToUniversalTime())
        ];

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<Operation?> Get(
        Guid id,
        OperationType operationType,
        CancellationToken cancellationToken)
    {
        Operation? operation = null;

        var tableName = operationType == OperationType.TopUp
            ? "top_ups"
            : "withdraws";

        var sql = $"""
            SELECT
                id,
                user_id,
                amount,
                type,
                status,
                created_at,
                updated_at
            FROM {tableName}
            WHERE id = @id;
            """;

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("id", id));

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var operationId = reader.GetGuid(reader.GetOrdinal("id"));
            var userId = reader.GetInt64(reader.GetOrdinal("user_id"));
            var amount = reader.GetDecimal(reader.GetOrdinal("amount"));
            var type = (OperationType)reader[reader.GetOrdinal("type")];
            var status = (OperationStatus)reader[reader.GetOrdinal("status")];
            var createdAt = (DateTimeOffset)reader.GetDateTime(reader.GetOrdinal("created_at"));
            var updatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at"))
                ? null
                : (DateTimeOffset?)reader.GetDateTime(reader.GetOrdinal("updated_at"));

            operation = new Operation(
                id: operationId,
                userId: userId,
                amount: amount,
                type: type,
                status: status,
                createdAt: createdAt,
                updatedAt: updatedAt
            );
        }

        return operation;
    }

    public async Task Update(
        Operation operation,
        CancellationToken cancellationToken)
    {
        var tableName = operation.Type == OperationType.TopUp
            ? "top_ups"
            : "withdraws";

        var sql = $"""
            UPDATE {tableName}
            SET
                status = @status::operation_status,
                updated_at = @updated_at
            WHERE
                id = @id;
            """;

        NpgsqlParameter[] parameters =
        [
            new("id", operation.Id),
            new("status", operation.Status),
            new("updated_at", operation.UpdatedAt?.ToUniversalTime())
        ];

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

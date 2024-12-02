using Microsoft.Extensions.Logging;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Users;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Infra.Repositories;

internal sealed class UserRepository : BaseRepository, IUsersRepository
{
    public UserRepository(
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        ILogger<BaseRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public async Task<long?> Create(
        User user,
        CancellationToken cancellationToken)
    {
        const string sql = """
            insert into
                users(id, balance)
            values
                (@id, @balance)
            on conflict (id) do nothing
            returning id;
            """;

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add("id", user.Id);
        command.Parameters.Add("balance", user.Balance);

        object? insertedId = await command.ExecuteScalarAsync(cancellationToken);
        return insertedId as long?;
    }

    /// <summary>
    /// Метод создан для того чтобы потренироваться в работе с IAsyncEnumerable
    /// </summary>
    public async IAsyncEnumerable<long> Create(
        User[] users,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
            insert into
                users(id, balance_id)
            select
                id,
                balance
            from
                unnest(@ids, @balances) as d(id, balance)
            returning id;
            """;

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);

        var ids = users.Select(u => u.Id).ToArray();
        var balances = users.Select(u => u.Balance).ToArray();

        command.Parameters.Add("ids", ids);
        command.Parameters.Add("balances", balances);

        using var reader = await command.ExecuteReaderAsync(
            CommandBehavior.Default,
            cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return reader.GetFieldValue<long>(ordinal: 0);
        }
    }

    public async Task<User?> Get(
        long id,
        CancellationToken cancellationToken)
    {
        User? user = null;

        const string sql = """
            select id, balance
            from users
            where id = @id;
            """;

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("id", id);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var userId = reader.GetInt64(reader.GetOrdinal("id"));
            var balance = reader.GetDecimal(reader.GetOrdinal("balance"));

            user = new User(userId, balance);
        }

        return user;
    }

    public async Task<int> Update(
        User user,
        CancellationToken cancellationToken)
    {
        const string sql = """
            update users
            set balance = @balance
            where id = @id;
            """;

        var connection = await _connectionFactory.GetConnection(cancellationToken);

        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("id", user.Id);
        command.Parameters.AddWithValue("balance", user.Balance);

        int affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows;
    }
}

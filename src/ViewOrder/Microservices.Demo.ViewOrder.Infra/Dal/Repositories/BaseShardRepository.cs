using Npgsql;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure.Connection;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Rules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Infra.Dal.Repositories;

internal class BaseShardRepository<TShardKey> : IDisposable
{
    protected const int DefaultTimeoutInSeconds = 5;

    private readonly IShardConnectionFactory _connectionFactory;
    private readonly IShardingRule<TShardKey> _shardingRule;

    protected NpgsqlDataSource? _dataSource;

    public BaseShardRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<TShardKey> shardingRule)
    {
        _connectionFactory = connectionFactory;
        _shardingRule = shardingRule;
    }

    protected async Task<ShardNpgsqlConnection> GetOpenedConnectionByShardKey(
        TShardKey shardKey,
        CancellationToken cancellationToken)
    {
        var bucketId = _shardingRule.GetBucketId(shardKey);

        var connection = GetConnectionByBucketId(bucketId);

        return await OpenConnectionIfNeeded(
            connection,
            cancellationToken);
    }

    protected async Task<ShardNpgsqlConnection> GetOpenedConnectionByBucket(
        int bucketId,
        CancellationToken cancellationToken)
    {
        var connection = GetConnectionByBucketId(bucketId);

        return await OpenConnectionIfNeeded(
            connection,
            cancellationToken);
    }

    protected IEnumerable<int> AllBuckets =>
        _connectionFactory.GetAllBuckets();

    private ShardNpgsqlConnection GetConnectionByBucketId(int bucketId)
    {
        var connectionString = _connectionFactory.GetConnectionString(bucketId);

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        MapCompositTypes(dataSourceBuilder);
        _dataSource = dataSourceBuilder.Build();

        var pgConnection = _dataSource.CreateConnection();

        return new ShardNpgsqlConnection(pgConnection, bucketId);
    }

    private async Task<ShardNpgsqlConnection> OpenConnectionIfNeeded(
        ShardNpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        if (connection.State is ConnectionState.Closed)
        {
            await connection.OpenAsync(cancellationToken);
        }

        return connection;
    }

    private void MapCompositTypes(NpgsqlDataSourceBuilder builder)
    {
        builder.MapComposite<OrderDalModel>("order_v1");
    }

    public void Dispose()
    {
        _dataSource?.Dispose();
    }
}

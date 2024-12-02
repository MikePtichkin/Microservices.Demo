using Dapper;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure.Connection;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Rules;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Infra.Dal.Repositories;

internal sealed class OrderRepository : BaseShardRepository<long>, IOrderRepository
{
    public OrderRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<long> shardingRule)
        : base(connectionFactory, shardingRule)
    {
    }

    public async Task Add(
        OrderDalModel order,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = """
            insert into __bucket__.orders (order_id, region_id, status, customer_id, comment, created_at)
            values (
                @OrderId,
                @RegionId,
                @Status,
                @CustomerId,
                @Comment,
                @CreatedAt);
            """;

        var cmd = new CommandDefinition(
            sql,
            new
            {
                OrderId = order.OrderId,
                RegionId = order.RegionId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                Comment = order.Comment,
                CreatedAt = order.CreatedAt,
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await GetOpenedConnectionByShardKey(
            order.OrderId,
            token);

        await connection.ExecuteAsync(cmd);
    }

    public async Task Update(
        OrderDalModel order,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = """
            update __bucket__.orders
            set region_id = o.region_id,
                status = o.status,
                customer_id = o.customer_id,
                comment = o.comment,
                created_at = o.created_at
            from unnest(@Orders::order_v1[]) AS o(order_id, region_id, status, customer_id, comment, created_at)
            where __bucket__.orders.order_id = o.order_id;
            """;

        var cmd = new CommandDefinition(
            sql,
            new
            {
                Orders = new[] { order }
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await GetOpenedConnectionByShardKey(
            order.OrderId,
            token);

        await connection.ExecuteAsync(cmd);
    }

    public async Task<IReadOnlyCollection<OrderDalModel>> Query(
        OrdersQuery query,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var sql = """
            select order_id,
                   region_id,
                   status,
                   customer_id,
                   comment,
                   created_at
            from __bucket__.orders
            where 1 = 1
            """;

        if (query.OrderIds.Any())
        {
            sql += " and order_id = any(@OrderIds)";
        }
        if (query.CustomerIds.Any())
        {
            sql += " and customer_id = any(@CustomerIds)";
        }
        if (query.RegionIds.Any())
        {
            sql += " and region_id = any(@RegionIds)";
        }

        sql += " limit @Limit offset @Offset";

        var cmd = new CommandDefinition(
            sql,
            new
            {
                OrderIds = query.OrderIds,
                CustomerIds = query.CustomerIds,
                RegionIds = query.RegionIds,
                Limit = query.Limit,
                Offset = query.Offset
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        var result = new List<OrderDalModel>();
        foreach (var bucket in AllBuckets)
        {
            await using var connection = await GetOpenedConnectionByBucket(
                bucket,
                token);

            var ordersInBucket = await connection.QueryAsync<OrderDalModel>(cmd);

            result.AddRange(ordersInBucket);
        }

        return result;
    }
}

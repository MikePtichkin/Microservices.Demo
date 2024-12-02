using Microsoft.Extensions.Options;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.ReportService.Domain.Orders;
using Microservices.Demo.ReportService.Infra.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderStatusDomain = Microservices.Demo.ReportService.Domain.Orders.OrderStatus;

namespace Microservices.Demo.ReportService.Infra.Clients.Grpc;

internal sealed class OrdersGrpcClient : IOrdersCient
{
    private readonly OrderGrpc.OrderGrpcClient _ordersClient;
    private readonly int _maxConcurrentRequests;

    public OrdersGrpcClient(
        OrderGrpc.OrderGrpcClient ordersClient,
        IOptions<OrderServiceOptions> options)
    {
        _ordersClient = ordersClient;
        _maxConcurrentRequests = options.Value.MaxConcurrentRequests;
    }

    public async Task<Order[]> Query(long customersId, CancellationToken token)
    {
        var totalCount = await GetOrdersTotalCount(customersId, token);

        var batchSize = GetOptimalBatchSize(totalCount);

        List<Task<Order[]>> tasks = [];
        for (var offset = 0; offset < totalCount; offset += batchSize)
        {
            var request = new V1QueryOrdersRequest()
            {
                CustomerIds = { customersId },
                Limit = batchSize,
                Offset = offset
            };

            tasks.Add(QueryOrders(request, token));
        }

        var results = await Task.WhenAll(tasks);

        var orderEntities = results
            .SelectMany(o => o)
            .ToArray();

        return orderEntities;
    }

    private int GetOptimalBatchSize(long totalCount)
    {
        var batchSize = (int)(totalCount / _maxConcurrentRequests);

        var batcHasRemainder = totalCount % _maxConcurrentRequests > 0;

        return batcHasRemainder
            ? batchSize + 1
            : batchSize;
    }

    private async Task<long> GetOrdersTotalCount(long customerId, CancellationToken token)
    {
        var request = new V1QueryOrdersRequest()
        {
            CustomerIds = { customerId },
            OrderIds = { },
            RegionIds = { },
            Limit = 1,
            Offset = 0
        };

        using var call = _ordersClient.V1QueryOrders(request, cancellationToken: token);
        var responseStream = call.ResponseStream;

        while (await responseStream.MoveNext(token))
        {
            return responseStream.Current.TotalCount;
        }

        return 0;
    }

    private async Task<Order[]> QueryOrders(
        V1QueryOrdersRequest request,
        CancellationToken token)
    {
        using var call = _ordersClient.V1QueryOrders(request, cancellationToken: token);
        var responseStream = call.ResponseStream;
        var orders = new List<Order>();

        while (await responseStream.MoveNext(token))
        {
            var response = responseStream.Current;

            var orderDomain = new Order(
                id: response.OrderId,
                status: (OrderStatusDomain)response.Status,
                createdAt: response.CreatedAt,
                comment: response.Comment);

            orders.Add(orderDomain);
        }

        return [.. orders];
    }
}

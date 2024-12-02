using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrderStatusDomain = Microservices.Demo.OrdersFacade.Domain.Orders.OrderStatus;

namespace Microservices.Demo.OrdersFacade.Infra.Clients.Grpc;

internal sealed class OrdersGrpcClients : IOrdersCient
{
    private readonly OrderGrpc.OrderGrpcClient _ordersClient;

    public OrdersGrpcClients(OrderGrpc.OrderGrpcClient orderClient)
    {
        _ordersClient = orderClient;
    }

    public async Task<Order[]> Query(
        long[] customersIds,
        long[] ordersIds,
        long[] regionId,
        int limit,
        int offset,
        CancellationToken token)
    {
        var request = new V1QueryOrdersRequest()
        {
            CustomerIds = { customersIds },
            OrderIds = { ordersIds },
            RegionIds = { regionId },
            Limit = limit,
            Offset = offset
        };

        using var call = _ordersClient.V1QueryOrders(request, cancellationToken: token);
        var responseStream = call.ResponseStream;
        var orders = new List<Order>();

        while (await responseStream.MoveNext(token))
        {
            var response = responseStream.Current;

            var orderDomain = new Order(
                id: response.OrderId,
                regionId: response.Region.Id,
                status: (OrderStatusDomain)response.Status,
                customerId: response.CustomerId,
                createdAt: response.CreatedAt.ToDateTime(),
                comment: new OrderComment(response.Comment));

            orders.Add(orderDomain);
        }

        return [.. orders];
    }
}

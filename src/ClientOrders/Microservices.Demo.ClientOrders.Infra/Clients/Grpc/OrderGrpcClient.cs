using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Order = Microservices.Demo.ClientOrders.Bll.Models.Order;

namespace Microservices.Demo.ClientOrders.Infra.Clients.Grpc;

internal sealed class OrderGrpcClient : IOrderClient
{
    private readonly OrderGrpc.OrderGrpcClient _orderClient;

    public OrderGrpcClient(OrderGrpc.OrderGrpcClient orderClient)
    {
        _orderClient = orderClient;
    }

    public async Task<Order?> Query(
        long orderId,
        CancellationToken token)
    {
        var request = new V1QueryOrdersRequest()
        {
            OrderIds = { orderId },
            Limit = 1,
            Offset = 0
        };

        using var call = _orderClient.V1QueryOrders(request, cancellationToken: token);
        var responseStream = call.ResponseStream;
        var orders = new List<Order>();

        while (await responseStream.MoveNext(token))
        {
            var response = responseStream.Current;

            var orderModel = new Order(
                OrderId: response.OrderId,
                CustomerId: response.CustomerId,
                RegionId: response.Region.Id,
                RegionName: response.Region.Name,
                Status: response.Status,
                Comment: response.Comment,
                CreatedAt: response.CreatedAt.ToDateTimeOffset());

            orders.Add(orderModel);
        }

        return orders.FirstOrDefault();
    }
}

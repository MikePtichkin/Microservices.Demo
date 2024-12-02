using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Infra.Clients.Grpc;

internal sealed class OrderGrpcClient : IOrderClient
{
    private readonly OrderGrpc.OrderGrpcClient _orderGrpcClient;

    public OrderGrpcClient(OrderGrpc.OrderGrpcClient orderGrpcClient)
    {
        _orderGrpcClient = orderGrpcClient;
    }

    public async Task<OrderClientModel?> Query(
        long orderId,
        CancellationToken token)
    {
        var request = new V1QueryOrdersRequest()
        {
            OrderIds = { orderId },
            Limit = 1,
            Offset = 0,
        };

        using var call = _orderGrpcClient.V1QueryOrders(request, cancellationToken: token);
        var responseStream = call.ResponseStream;
        var orders = new List<OrderClientModel>();

        while (await responseStream.MoveNext(token))
        {
            var response = responseStream.Current;

            var orderModel = new OrderClientModel(
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

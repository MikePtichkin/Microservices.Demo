using Grpc.Core;
using MediatR;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.GetCustomerOrders;
using Microservices.Demo.ClientOrders.Features.Orders.Grpc.Mappers;
using Microservices.Demo.ClientOrders.Protos;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Features.Orders.Grpc;

public class OrdersGrpcService : Protos.OrdersGrpcService.OrdersGrpcServiceBase
{
    private readonly ISender _sender;

    public OrdersGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public async override Task<V1CreateOrderResponse> V1CreateOrder(
        V1CreateOrderRequest request,
        ServerCallContext context)
    {
        var createOrderCommand = new CreateOrderCommand(
            CustomerId: request.CustomerId,
            StockItems: request.Items.Select(item => item.ToDomain()).ToArray());

        await _sender.Send(createOrderCommand, context.CancellationToken);

        return new V1CreateOrderResponse();
    }

    public override async Task<V1QueryOrdersResponse> V1QueryOrders(
        V1QueryOrdersRequest request,
        ServerCallContext context)
    {
        var customerOrdersQuery = new GetCustomerOrdersQuery(
            request.CustomerId,
            request.Limit,
            request.Offset);

        var customerOrders = await _sender.Send(customerOrdersQuery, context.CancellationToken);

        return new V1QueryOrdersResponse
        {
            OrderInfo = { customerOrders.Orders.Select(o => o.ToProto()) }
        };
    }
}

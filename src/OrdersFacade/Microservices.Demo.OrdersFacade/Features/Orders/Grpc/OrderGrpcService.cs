using Grpc.Core;
using MediatR;
using Microservices.Demo.OrdersFacade.Application.Orders.GetCustomerOrders;
using Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;
using Microservices.Demo.OrdersFacade.Features.Orders.Grpc.Mappers;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Grpc;

public class OrderGrpcService : OrdersFacadeGrpc.OrdersFacadeGrpcBase
{
    private readonly ISender _sender;

    public OrderGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<V1QueryOrdersByCustomerResponse> V1QueryOrdersByCustomer(
        V1QueryOrdersByCustomerRequest request,
        ServerCallContext context)
    {
        var ordersByCustomerQuery = new GetCustomerOrdersQuery(
            CustomerId: request.CustomerId,
            Limit: request.Limit,
            Offset: request.Offset);

        var ordersByCustomerResult = await _sender.Send(
            ordersByCustomerQuery,
            context.CancellationToken);

        var customerDomain = ordersByCustomerResult.Value.Customer;
        var ordersDomain = ordersByCustomerResult.Value.Orders;

        var response = new V1QueryOrdersByCustomerResponse()
        {
            Customer = customerDomain.ToGrpcCustomer()
        };

        response.Orders.AddRange(ordersDomain.Select(V1QueryOrdersByCustomerMapper.ToGrpcOrder));

        return response;
    }

    public override async Task<V1QueryOrdersByRegionResponse> V1QueryOrdersByRegion(
        V1QueryOrdersByRegionRequest request,
        ServerCallContext context)
    {
        var orderByRegionQuery = new GetRegionOrdersQuery(
            RegionId: request.RegionId,
            Limit: request.Limit,
            Offset: request.Offset);

        var ordersByRegionResult = await _sender.Send(orderByRegionQuery, context.CancellationToken);

        var response = new V1QueryOrdersByRegionResponse();
        response.Orders.AddRange(ordersByRegionResult.Value.OrdersWithCustomerNames.Select(V1QueryOrdersByRegionMapper.ToGrpcOrder));

        return response;
    }
}

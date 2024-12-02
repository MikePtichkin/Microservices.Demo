using Google.Protobuf.WellKnownTypes;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Grpc.Mappers;

public static class V1QueryOrdersByCustomerMapper
{
    public static V1QueryOrdersByCustomerResponse.Types.Order ToGrpcOrder(this Order orderDomain) => new()
    {
        OrderId = orderDomain.Id,
        RegionId = orderDomain.RegionId,
        Status = (OrderStatus)orderDomain.Status,
        CustomerId = orderDomain.CustomerId,
        Comment = orderDomain.Comment?.Value,
        CreatedAt = orderDomain.CreatedAt.ToTimestamp()
    };

    public static V1QueryOrdersByCustomerResponse.Types.Customer ToGrpcCustomer(this Customer customerDomain) => new()
    {
        CustomerId = customerDomain.Id,
        RegionId = customerDomain.RegionId,
        FullName = customerDomain.FullName.Value,
        CreatedAt = customerDomain.CreatedAt.ToTimestamp()
    };
}

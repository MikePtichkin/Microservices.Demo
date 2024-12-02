using Google.Protobuf.WellKnownTypes;
using Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;
using Microservices.Demo.OrdersFacade.Domain.Orders;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Grpc.Mappers;

public static class V1QueryOrdersByRegionMapper
{
    public static V1QueryOrdersByRegionResponse.Types.Order ToGrpcOrder(this OrderWithCustomerInfoDto orderDto) => new()
    {
        OrderId = orderDto.OrderId,
        RegionId = orderDto.RegionId,
        Status = (OrderStatus)orderDto.Status,
        CustomerId = orderDto.CustomerId,
        CustomerName = orderDto.CustomerName,
        Comment = orderDto.Comment?.Value,
        CreatedAt = orderDto.CreatedAt.ToTimestamp()
    };

}

using Microservices.Demo.ClientOrders.Bll.Orders.Features.GetCustomerOrders;
using Microservices.Demo.ClientOrders.Protos;

namespace Microservices.Demo.ClientOrders.Features.Orders.Grpc.Mappers;

public static class V1QueryOrdersMapper
{
    public static V1QueryOrdersResponse.Types.OrderInfo ToProto(
        this OrderInfo orderInfo) => new()
        {
            OrderId = orderInfo.OrderId,
            Status = (OrderStatus)orderInfo.Status,
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(orderInfo.CreatedAt)
        };
    
}

using Microservices.Demo.ClientOrders.Domain.Orders;
using Microservices.Demo.ClientOrders.Protos;

namespace Microservices.Demo.ClientOrders.Features.Orders.Grpc.Mappers;

public static class V1CreateOrderMapper
{
    public static StockItem ToDomain(
        this V1CreateOrderRequest.Types.Item item) => new(
            barcode: item.Barcode,
            quantity: item.Quantity);
    
}

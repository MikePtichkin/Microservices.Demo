namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;

public sealed record OrdersInputMessage
{
    public required long RegionId { get; init; }

    public required long CustomerId { get; init; }

    public string? Comment { get; init; }

    public required Item[] Items { get; init; }
}

public sealed record class Item
{
    public required string Barcode { get; init; }

    public required int Quantity { get; init; }
}

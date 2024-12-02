namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;

public sealed record OrdersInputErrorsMessage
{
    public required OrdersInputMessage InputOrder { get; init; }

    public required ErrorReason ErrorReason { get; init; }
}

public sealed record ErrorReason
{
    public required string Code { get; init; }

    public required string Text { get; init; }
}

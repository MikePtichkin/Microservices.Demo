namespace Microservices.Demo.ClientOrders.Domain.Orders;

public sealed record Barcode
{
    public Barcode()
    { }

    public required string Value { get; init; }

    public static implicit operator Barcode(string barcode) => new()
    {
        Value = barcode
    };

    public static implicit operator string(Barcode barcode) =>
        barcode.Value;
}



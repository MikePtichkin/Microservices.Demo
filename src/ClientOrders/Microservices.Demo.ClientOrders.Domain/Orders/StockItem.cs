namespace Microservices.Demo.ClientOrders.Domain.Orders;

public sealed record StockItem
{
    public StockItem(Barcode barcode, int quantity)
    {
        ItemBarcode = barcode;
        Quantity = quantity;
    }

    public Barcode ItemBarcode { get; init; }
    public int Quantity { get; init; }
}
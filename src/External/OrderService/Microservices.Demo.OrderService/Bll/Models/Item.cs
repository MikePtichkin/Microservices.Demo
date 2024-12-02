namespace Microservices.Demo.OrderService.Bll.Models;

public record Item(
    string Barcode,
    int Quantity);
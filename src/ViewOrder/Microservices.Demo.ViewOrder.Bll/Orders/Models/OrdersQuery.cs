namespace Microservices.Demo.ViewOrder.Bll.Orders.Models;

public record OrdersQuery(
    long[] OrderIds,
    long[] RegionIds,
    long[] CustomerIds,
    int Limit,
    int Offset);

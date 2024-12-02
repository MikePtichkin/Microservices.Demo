using Microservices.Demo.DataGenerator.Bll.Creators;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.Models.Enums;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;
using Random = Microservices.Demo.DataGenerator.Bll.Creators.Random;

namespace Microservices.Demo.DataGenerator.Bll.Services;

public class BrokenOrderService : IBrokenOrderService
{
    public IReadOnlyList<int> GetBrokenOrderIndexes(
        long totalOrdersCount,
        int invalidOrderCounterNumber,
        int ordersCountToCreate)
    {
        if (invalidOrderCounterNumber == 0)
        {
            return [];
        }

        var period = invalidOrderCounterNumber;

        var lastBrokenOrder = totalOrdersCount / period * period;

        var nextBrokenIndex = (int)(period - (totalOrdersCount - lastBrokenOrder) - 1);

        var brokenOrderIndexes = new List<int>(ordersCountToCreate);

        while (nextBrokenIndex < ordersCountToCreate)
        {
            brokenOrderIndexes.Add(nextBrokenIndex);
            nextBrokenIndex += period;
        }

        return brokenOrderIndexes;
    }

    public Order BreakOrder(Order order)
    {
        var brokenReason = Random.EnumValue<OrderBrokenReason>();

        return brokenReason switch
        {
            OrderBrokenReason.InvalidRegion => SetInvalidRegion(order),
            OrderBrokenReason.EmptyItems => SetEmptyItems(order),
            OrderBrokenReason.IncorrectItemsQuantity => SetIncorrectItemsQuantity(order),

            _ => throw new ArgumentOutOfRangeException(nameof(OrderBrokenReason))
        };
    }

    private static Order SetInvalidRegion(Order order)
        => order with { RegionId = Random.InvalidRegion };

    private static Order SetEmptyItems(Order order)
        => order with { Items = [] };

    private static Order SetIncorrectItemsQuantity(Order order)
        => order with { Items = ItemCreator.CreateInvalidItems() };
}

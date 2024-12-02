using Microservices.Demo.DataGenerator.Bll.Models;

namespace Microservices.Demo.DataGenerator.Bll.Services.Contracts;

public interface IBrokenOrderService
{
    IReadOnlyList<int> GetBrokenOrderIndexes(
        long totalOrdersCount,
        int invalidOrderCounterNumber,
        int ordersCountToCreate);

    Order BreakOrder(Order order);
}

using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Domain.Orders;

public interface IOrdersCient
{
    Task<Order[]> Query(
        long[] customersIds,
        long[] ordersIds,
        long[] regionIds,
        int limit,
        int offset,
        CancellationToken token);
}

using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Domain.Orders.Contracts;

public interface IOrdersRepository
{
    Task<Order?> Get(long id, CancellationToken token);

    Task<Order[]> GetCustomerOrders(
        long customerId,
        int limit,
        int offset,
        CancellationToken token);

    Task<int> Add(Order order, CancellationToken token);

    Task<int> Update(Order order, CancellationToken token);
}

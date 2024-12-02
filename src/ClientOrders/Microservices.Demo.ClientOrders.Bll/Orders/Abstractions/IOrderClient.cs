using System.Threading;
using System.Threading.Tasks;
using Order = Microservices.Demo.ClientOrders.Bll.Models.Order;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;

public interface IOrderClient
{
    Task<Order?> Query(long orderId, CancellationToken token);
}

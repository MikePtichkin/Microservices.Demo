using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;

public interface IOrderClient
{
    Task<OrderClientModel?> Query(long orderId, CancellationToken token);
}

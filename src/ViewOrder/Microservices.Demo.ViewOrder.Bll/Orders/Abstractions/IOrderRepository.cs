using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;

public interface IOrderRepository
{
    Task Add(OrderDalModel order, CancellationToken token);

    Task Update(OrderDalModel order, CancellationToken token);

    Task<IReadOnlyCollection<OrderDalModel>> Query(OrdersQuery query, CancellationToken token);
}

using Microservices.Demo.OrderService.Dal.Entities;
using Microservices.Demo.OrderService.Dal.Models;

namespace Microservices.Demo.OrderService.Dal.Interfaces;

public interface IOrdersRepository
{
    Task<long[]> Insert(
        OrderEntity order,
        CancellationToken token);

    Task Update(
        OrderEntity order,
        CancellationToken token);

    Task<OrderInfoEntity[]> Query(
        OrderQueryModel query,
        CancellationToken token);
}
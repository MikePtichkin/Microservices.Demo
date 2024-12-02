using Microservices.Demo.OrderService.Dal.Entities;

namespace Microservices.Demo.OrderService.Dal.Repositories;

public interface IItemsRepository
{
    Task Insert(
        ItemEntity[] items,
        CancellationToken token);
}
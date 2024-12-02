using Microsoft.EntityFrameworkCore;
using Microservices.Demo.ClientOrders.Domain.Orders;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;
using Microservices.Demo.ClientOrders.Infra.Dal.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Repositories;

internal sealed class OrdersRepository : IOrdersRepository
{
    private readonly ClientOrdersDbContext _dbContext;

    public OrdersRepository(ClientOrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Order?> Get(long id, CancellationToken token)
    {
        return _dbContext
            .Set<Order>()
            .FindAsync([id], token)
            .AsTask();
    }

    public Task<Order[]> GetCustomerOrders(
        long customerId,
        int limit,
        int offset,
        CancellationToken token)
    {
        return _dbContext.Set<Order>()
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToArrayAsync(token);
    }

    public Task<int> Add(Order order, CancellationToken token)
    {
        _dbContext.Set<Order>().Add(order);

        return _dbContext.SaveChangesAsync(token);
    }

    public Task<int> Update(Order order, CancellationToken token)
    {
        _dbContext.Set<Order>().Update(order);

        return _dbContext.SaveChangesAsync(token);
    }
}

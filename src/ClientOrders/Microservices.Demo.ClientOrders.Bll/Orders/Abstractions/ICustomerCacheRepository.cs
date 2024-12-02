using Microservices.Demo.ClientOrders.Domain.Customers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;

public interface ICustomerCacheRepository
{
    Task InsertCustomer(Customer customer, CancellationToken token);
    Task<Customer?> GetCustomer(long customerId, CancellationToken token);
}

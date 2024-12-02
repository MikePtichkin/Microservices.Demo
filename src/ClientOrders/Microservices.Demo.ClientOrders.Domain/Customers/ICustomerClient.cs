using System.Threading.Tasks;
using System.Threading;

namespace Microservices.Demo.ClientOrders.Domain.Customers;

public interface ICustomerClient
{
    Task<Customer?> Query(
        long customerId,
        CancellationToken token);
}

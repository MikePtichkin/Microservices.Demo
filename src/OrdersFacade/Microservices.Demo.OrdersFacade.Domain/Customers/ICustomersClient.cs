using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Domain.Customers;

public interface ICustomersClient
{
    Task<Customer[]> Query(
        long[] customerIds,
        int limit,
        int offset,
        CancellationToken token);
}

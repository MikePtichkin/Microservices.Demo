using Microservices.Demo.ReportService.Domain.Orders;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Domain.Orders;

public interface IOrdersCient
{
    Task<Order[]> Query(long customersId, CancellationToken token);
}

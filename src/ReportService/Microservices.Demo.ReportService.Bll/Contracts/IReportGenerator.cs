using Microservices.Demo.ReportService.Domain.Orders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Bll.Contracts;

public interface IReportGenerator
{
    Task<byte[]> GenerateReportCsvBytes(
        long customerId,
        CancellationToken cancellationToken);
}
using MediatR;
using Microservices.Demo.ReportService.Bll.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Bll.Reports.GetCsvCustomerOrdersReport;

internal sealed class GetCsvCustomerOrdersReportHandler
    : IRequestHandler<GetCsvCustomerOrdersReportQuery, byte[]>
{
    private readonly IRateLimiter _rateLimiter;
    private readonly IReportGenerator _reportGenerator;

    public GetCsvCustomerOrdersReportHandler(
        IRateLimiter rateLimiter,
        IReportGenerator reportGenerator)
    {
        _rateLimiter = rateLimiter;
        _reportGenerator = reportGenerator;
    }

    public async Task<byte[]> Handle(
        GetCsvCustomerOrdersReportQuery request,
        CancellationToken cancellationToken)
    {
        using var currentCts = new CancellationTokenSource();
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            currentCts.Token);

        var guid = Guid.NewGuid(); // GUID для отладки.

        _rateLimiter.CtsByCustomerId.AddOrUpdate(
            request.CustomerId,
            (linkedTokenSource, guid),
            (customerId, existingCts) =>
            {
                existingCts.Cts.Cancel();
                return (linkedTokenSource, guid);
            });

        try
        {
            await _rateLimiter.WaitAsync(linkedTokenSource.Token);

            var csvReport = await _reportGenerator.GenerateReportCsvBytes(request.CustomerId, linkedTokenSource.Token);

            _rateLimiter.CtsByCustomerId.TryRemove(request.CustomerId, out var _);

            return csvReport;
        }
        finally
        {
            _rateLimiter.Release();
        }
    }
}

using Microsoft.Extensions.Options;
using Microservices.Demo.ReportService.Bll.Contracts;
using Microservices.Demo.ReportService.Infra.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Infra.RateLimiter;

internal sealed class ReportGeneratorRateLimiter : IRateLimiter
{
    private readonly SemaphoreSlim _semaphore;

    private ConcurrentDictionary<long, (CancellationTokenSource Cts, Guid RequestId)> _ctsByCustomerId;

    public ReportGeneratorRateLimiter(IOptions<CsvReportGeneratorOptions> options)
    {
        var initialCount = options.Value.MaxConcurrentRequests;
        var maxCount = options.Value.MaxConcurrentRequests;
        _semaphore = new SemaphoreSlim(initialCount, maxCount);
        _ctsByCustomerId = new ConcurrentDictionary<long, (CancellationTokenSource Cts, Guid RequestId)>();
    }

    public int Release() => _semaphore.Release();

    public Task WaitAsync(CancellationToken cancellationToken) => _semaphore.WaitAsync(cancellationToken);
    
    public ConcurrentDictionary<long, (CancellationTokenSource Cts, Guid RequestId)> CtsByCustomerId => _ctsByCustomerId;
}

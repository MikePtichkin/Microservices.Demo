using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Bll.Contracts;

public interface IRateLimiter
{
    ConcurrentDictionary<long, (CancellationTokenSource Cts, Guid RequestId)> CtsByCustomerId { get; }
    Task WaitAsync(CancellationToken cancellationToken);
    int Release();
}

using Microservices.Demo.ReportService.Bll.Contracts;
using System.Collections.Concurrent;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Bll.UnitTests.Reports;

public class TestRateLimiter : IRateLimiter
{
    public ConcurrentDictionary<long, (CancellationTokenSource Cts, Guid RequestId)> CtsByCustomerId { get; }

    private int _waitAsyncCallCount;
    private int _releaseCallCount;

    public TestRateLimiter()
    {
        CtsByCustomerId = new ConcurrentDictionary<long, (CancellationTokenSource Cts, Guid RequestId)>();
    }

    public Task WaitAsync(CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _waitAsyncCallCount);
        return Task.CompletedTask;
    }

    public int Release()
    {
        return Interlocked.Increment(ref _releaseCallCount);
    }

    public void WaitAsyncCalledOnce()
    {
        Assert.Equal(1, _waitAsyncCallCount);
    }

    public void ReleaseCalledOnce()
    {
        Assert.Equal(1, _releaseCallCount);
    }
}

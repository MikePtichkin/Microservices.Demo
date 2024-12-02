using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Infrastructure.Middlewares;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTime.UtcNow;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var endTime = DateTime.UtcNow;

            _logger.LogInformation("Request {method} {url} started at {startTime} and ended at {endTime} completed in {duration} ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                startTime,
                endTime,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

using MediatR;
using Microservices.Demo.TestService.Data;
using Microservices.Demo.TestService.Domain.Metrics;
using Microsoft.Extensions.Logging;

namespace Microservices.Demo.TestService.Domain.Actions.ClearMismatches;

public class ClearMismatchesHandler : IRequestHandler<ClearMismatchesCommand>
{
    private readonly IMismatchRepository _mismatchRepository;
    private readonly IMismatchMetricReporter _mismatchMetricReporter;
    private readonly ILogger<ClearMismatchesHandler> _logger;

    public ClearMismatchesHandler(
        IMismatchRepository mismatchRepository,
        IMismatchMetricReporter mismatchMetricReporter,
        ILogger<ClearMismatchesHandler> logger)
    {
        _mismatchRepository = mismatchRepository;
        _mismatchMetricReporter = mismatchMetricReporter;
        _logger = logger;
    }

    public Task Handle(ClearMismatchesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Clearing mismatch storage...");

        _mismatchRepository.Clear();
        _mismatchMetricReporter.Clear();

        _logger.LogInformation("Mismatch storage cleared.");

        return Task.CompletedTask;
    }
}

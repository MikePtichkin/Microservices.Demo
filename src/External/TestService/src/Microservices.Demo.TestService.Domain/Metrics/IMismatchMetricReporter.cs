using Microservices.Demo.TestService.Data;

namespace Microservices.Demo.TestService.Domain.Metrics;

public interface IMismatchMetricReporter
{
    void Inc(MismatchType mismatchType);

    void Clear();
}

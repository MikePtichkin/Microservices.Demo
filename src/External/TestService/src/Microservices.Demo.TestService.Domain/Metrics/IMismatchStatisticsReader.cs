using Microservices.Demo.TestService.Data;

namespace Microservices.Demo.TestService.Domain.Metrics;

public interface IMismatchStatisticsReader
{
    IReadOnlyDictionary<MismatchType, int> GetStatistics();
}

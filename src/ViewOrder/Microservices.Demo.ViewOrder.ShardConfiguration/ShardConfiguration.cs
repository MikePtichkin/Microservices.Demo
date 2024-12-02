using System.Collections.Generic;

namespace Microservices.Demo.ViewOrder.ShardConfiguration;

public record ShardConfiguration
{
    public required int BucketsCount { get; init; }
    
    public required IReadOnlyList<DbEndpoint> Endpoints { get; init; }
}
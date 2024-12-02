using Microservices.Demo.ViewOrder.ShardConfiguration;
using System.Collections.Generic;

namespace Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure;

public interface IDbStore
{
    IReadOnlyList<DbEndpoint> GetAllEndpoints();

    DbEndpoint GetEndpointByBucket(int bucketId);

    int BucketsCount { get; }
}

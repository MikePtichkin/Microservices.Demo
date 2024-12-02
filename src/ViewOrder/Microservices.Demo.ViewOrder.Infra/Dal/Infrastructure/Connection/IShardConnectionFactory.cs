using System.Collections.Generic;

namespace Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure.Connection;

public interface IShardConnectionFactory
{
    string GetConnectionString(int bucketId);

    IEnumerable<int> GetAllBuckets();
}

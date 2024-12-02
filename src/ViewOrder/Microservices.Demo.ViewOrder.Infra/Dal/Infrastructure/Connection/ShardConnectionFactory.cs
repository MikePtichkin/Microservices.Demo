using Npgsql;
using System.Collections.Generic;

namespace Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure.Connection;

public class ShardConnectionFactory : IShardConnectionFactory
{
    private readonly IDbStore _dbStore;
    private readonly HashSet<string> _reloadedConnections = [];

    public ShardConnectionFactory(
        IDbStore dbStore)
    {
        _dbStore = dbStore;
    }

    public IEnumerable<int> GetAllBuckets()
    {
        for (int bucketId = 0; bucketId < _dbStore.BucketsCount; bucketId++)
        {
            yield return bucketId;
        }
    }

    public string GetConnectionString(int bucketId)
    {
        var endpoint = _dbStore.GetEndpointByBucket(bucketId);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = endpoint.ConnectionString.HostAndPort,
            Database = endpoint.ConnectionString.Database,
            Username = endpoint.ConnectionString.User,
            Password = endpoint.ConnectionString.Password
        };
        return builder.ToString();
    }
}

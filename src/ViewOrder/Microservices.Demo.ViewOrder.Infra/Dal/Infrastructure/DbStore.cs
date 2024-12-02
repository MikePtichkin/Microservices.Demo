using Microservices.Demo.ViewOrder.ShardConfiguration;
using System;
using System.Collections.Generic;

namespace Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure;

public class DbStore : IDbStore
{
    private readonly DbEndpoint[] _endpoints;
    private readonly Dictionary<long, DbEndpoint> _bucketEndpoints;

    public DbStore(DbEndpoint[] dbEndpoints)
    {
        _bucketEndpoints = [];
        int bucketsCount = 0;

        foreach (var endpoint in dbEndpoints)
        {
            foreach (var bucket in endpoint.Buckets)
            {
                _bucketEndpoints.Add(bucket, endpoint);
                bucketsCount++;
            }
        }

        _endpoints = dbEndpoints;
        BucketsCount = bucketsCount;
    }

    public int BucketsCount { get; private set; }

    public IReadOnlyList<DbEndpoint> GetAllEndpoints() =>
        _endpoints;

    public DbEndpoint GetEndpointByBucket(int bucketId)
    {
        if (!_bucketEndpoints.TryGetValue(bucketId, out DbEndpoint? bucket))
        {
            throw new ArgumentOutOfRangeException($"Bucket with id {bucketId} not found");
        }

        return bucket;
    }
}
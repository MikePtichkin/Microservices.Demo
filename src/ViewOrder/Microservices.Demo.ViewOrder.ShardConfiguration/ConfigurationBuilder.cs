using System;
using System.Linq;

namespace Microservices.Demo.ViewOrder.ShardConfiguration;

public static class ConfigurationBuilder
{
    public static ShardConfiguration Build(int bucketsPerShard, params string[] connectionStrings)
    {
        if (bucketsPerShard < 1 || bucketsPerShard > 101)
        {
            throw new ArgumentOutOfRangeException($"Допустимое число бакетов на шард 1-100. Текущее {bucketsPerShard}");
        }

        var bucketsCount = bucketsPerShard * connectionStrings.Length;

        var buckets = Enumerable.Range(0, bucketsCount)
            .Chunk(bucketsPerShard)
            .ToArray();

        var endpoints = connectionStrings
            .Select((t, i) => new DbEndpoint(t, buckets[i]))
            .ToList();

        return new ShardConfiguration()
        {
            BucketsCount = bucketsCount,
            Endpoints = endpoints
        };
    }
}
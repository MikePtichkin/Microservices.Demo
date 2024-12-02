namespace Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Rules;

public interface IShardingRule<TShardKey>
{
    int GetBucketId(TShardKey shardKey);
}
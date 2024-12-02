using Microservices.Demo.ViewOrder.ShardConfiguration;

namespace Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;

public interface IBucketMigrationContext
{
    public string CurrentDbSchema { get; }

    public int CurrentBucketId { get; }

    public DbEndpoint CurrentEndpoint { get; }
}

namespace Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Settings;

public record ShardedMigratorSettings
{
    public required string ConnectionStringShard1 { get; init; }
    public required string ConnectionStringShard2 { get; init; }
    public int BucketsPerShard { get; init; }
}

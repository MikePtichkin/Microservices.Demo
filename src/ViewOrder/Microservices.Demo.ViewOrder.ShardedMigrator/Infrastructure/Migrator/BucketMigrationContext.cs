using Microservices.Demo.ViewOrder.ShardConfiguration;

namespace Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;

public class BucketMigrationContext : IBucketMigrationContext
{
    private readonly DbEndpoint _currentShard;
    
    private string _currentDbSchema = string.Empty;
    private int _currentBucketId = 0;


    public BucketMigrationContext(
        DbEndpoint shardEndpoint,
        int bucketId)
    {
        _currentShard = shardEndpoint;
        _currentBucketId = bucketId;
        _currentDbSchema = ShardsHelper.GetSchemaName(bucketId);
    }

    public string CurrentDbSchema => _currentDbSchema;
    
    public int CurrentBucketId => _currentBucketId;

    public DbEndpoint CurrentEndpoint => _currentShard;
}
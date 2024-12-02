namespace Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure;

public static class ShardsHelper
{
    public const string BucketPlaceholder = "__bucket__";
    public static string GetSchemaName(int bucketId) => $"bucket_{bucketId}";
}
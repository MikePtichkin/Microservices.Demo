namespace Microservices.Demo.ViewOrder.ShardConfiguration;

public sealed record DbEndpoint
{
    public DbEndpoint(string connectionString, int[] buckets)
    {
        ConnectionString = ConnectionStringParser.Parse(connectionString);
        Buckets = buckets;
    }
    public ConnectionStringPostgres ConnectionString { get; init; }
    public  int[] Buckets { get; init; }
}

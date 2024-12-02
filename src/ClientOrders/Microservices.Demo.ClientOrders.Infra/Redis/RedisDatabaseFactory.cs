using StackExchange.Redis;
using System;
using System.Linq;

namespace Microservices.Demo.ClientOrders.Infra.Redis;

public sealed class RedisDatabaseFactory : IRedisDatabaseFactory, IDisposable
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisDatabaseFactory(string connectionString)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
    }

    public IDatabase GetDatabase()
    {
        return _connectionMultiplexer.GetDatabase();
    }

    public IServer GetServer()
    {
        var endponts = _connectionMultiplexer.GetEndPoints();
        return _connectionMultiplexer.GetServer(endponts.First());
    }

    public void Dispose()
    {
        _connectionMultiplexer.Dispose();
    }
}

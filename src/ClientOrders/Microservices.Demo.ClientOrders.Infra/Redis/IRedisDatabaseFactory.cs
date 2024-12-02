using StackExchange.Redis;

namespace Microservices.Demo.ClientOrders.Infra.Redis;

public interface IRedisDatabaseFactory
{
    IDatabase GetDatabase();
    IServer GetServer();
}

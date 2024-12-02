using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Domain.Customers;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infra.Redis.Repositories;

public sealed class CustomerRedisRepository : ICustomerCacheRepository
{
    private readonly IDatabase _redisDatabase;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public CustomerRedisRepository(IRedisDatabaseFactory redisDatabaseFactory)
    {
        _redisDatabase = redisDatabaseFactory.GetDatabase();
    }

    private static string GetKey(long customerId) => $"customer:{customerId}";

    public Task InsertCustomer(Customer customer, CancellationToken token)
    {
        var redisValue = JsonSerializer.Serialize(customer, _jsonSerializerOptions);

        return _redisDatabase.StringSetAsync(
            GetKey(customer.Id),
            redisValue,
            when: When.Always
        ).WaitAsync(token);
    }

    public async Task<Customer?> GetCustomer(long customerId, CancellationToken token)
    {
        var value = await _redisDatabase
            .StringGetAsync(GetKey(customerId))
            .WaitAsync(token);

        return value.HasValue
            ? JsonSerializer.Deserialize<Customer>(value, _jsonSerializerOptions)
            : null;
    }
}

using Microservices.Demo.DataGenerator.Bll.Creators;
using Microservices.Demo.DataGenerator.Bll.Models;
using Random = Microservices.Demo.DataGenerator.Bll.Creators.Random;

namespace Microservices.Demo.DataGenerator.Bll.Services.Contracts;

public interface IOrderService
{
    Order Create(IReadOnlyList<Customer> customers)
    {
        var customer = Random.Element(customers);

        var order = OrderCreator.Create() with
        {
            CustomerId = customer.Id!.Value,
            RegionId = customer.RegionId,
            Items = ItemCreator.Create(Random.ItemsCount)
        };

        return order;
    }
}

using Bogus;
using Microservices.Demo.DataGenerator.Bll.Models;

namespace Microservices.Demo.DataGenerator.Bll.Creators;

public static class OrderCreator
{
    private static readonly Faker<Order> Faker = new Faker<Order>()
        .RuleFor(order => order.Comment, faker => faker.Random.Words(12));

    public static Order Create() => Faker.Generate();
}

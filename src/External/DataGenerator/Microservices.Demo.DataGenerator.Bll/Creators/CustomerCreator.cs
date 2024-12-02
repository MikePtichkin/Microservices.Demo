using Bogus;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.Models.Enums;

namespace Microservices.Demo.DataGenerator.Bll.Creators;

public static class CustomerCreator
{
    private static readonly Faker<Customer> Faker = new Faker<Customer>()
        .RuleFor(customer => customer.RegionId, faker => (int)faker.Random.Enum<Region>())
        .RuleFor(customer => customer.FullName, faker => faker.Name.FullName());

    public static IReadOnlyList<Customer> Create(int count = 1) => Faker.Generate(count);
}

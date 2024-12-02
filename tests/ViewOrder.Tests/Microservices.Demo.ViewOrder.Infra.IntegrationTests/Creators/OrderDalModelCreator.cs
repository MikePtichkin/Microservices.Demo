using AutoBogus;
using Bogus;
using FluentAssertions.Common;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;

namespace Microservices.Demo.ViewOrder.Infra.UnitTests.Creators;

public static class OrderDalModelCreator
{
    private static readonly Faker<OrderDalModel> Faker = new AutoFaker<OrderDalModel>()
        .RuleFor(x => x.OrderId, f => f.Random.Long(10000))
        .RuleFor(x => x.RegionId, f => f.Random.Long(1, 3))
        .RuleFor(x => x.CustomerId, f => f.Random.Long(1, 10000))
        .RuleFor(x => x.Status, f => (int)Domain.Orders.OrderStatus.New)
        .RuleFor(x => x.CreatedAt, f => f.Date.Past().ToDateTimeOffset())
        .RuleFor(x => x.Comment, f => f.Lorem.Sentence());

    public static OrderDalModel Generate() =>
        Faker.Generate();

    public static OrderDalModel WithComment(this OrderDalModel src, string comment) =>
        src with
        {
            Comment = comment
        };

    public static OrderDalModel WithId(this OrderDalModel src, long id) =>
        src with
        {
            OrderId = id
        };

}

using AutoBogus;
using Bogus;
using FluentAssertions.Common;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;

namespace Microservices.Demo.ViewOrder.Bll.UnitTests.Creators;

public static class OrderClientModelCreator
{
    private static readonly Faker<OrderClientModel> Faker = new AutoFaker<OrderClientModel>()
        .RuleFor(x => x.OrderId, f => f.Random.Long(10000))
        .RuleFor(x => x.RegionId, f => f.Random.Long(1, 3))
        .RuleFor(x => x.CustomerId, f => f.Random.Long(1, 10000))
        .RuleFor(x => x.Status, f => (OrderStatus)f.Random.Int(1, 3))
        .RuleFor(x => x.CreatedAt, f => f.Date.Past().ToDateTimeOffset())
        .RuleFor(x => x.Comment, f => f.Lorem.Sentence())
        .RuleFor(x => x.RegionName, f => f.Address.City());

    public static OrderClientModel Generate() =>
        Faker.Generate();

    public static OrderClientModel WithComment(this OrderClientModel src, string? comment) =>
        src with
        {
            Comment = comment
        };

    public static OrderClientModel WithId(this OrderClientModel src, long id) =>
        src with
        {
            OrderId = id
        };

    public static OrderClientModel WithRegionName(this OrderClientModel src, string regionName) =>
        src with
        {
            RegionName = regionName
        };
}

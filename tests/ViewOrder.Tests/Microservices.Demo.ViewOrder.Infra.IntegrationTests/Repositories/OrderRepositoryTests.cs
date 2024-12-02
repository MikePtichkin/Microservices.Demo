using FluentAssertions;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using Microservices.Demo.ViewOrder.Infra.UnitTests.Creators;
using Microservices.Demo.ViewOrder.Infra.UnitTests.Fixtures;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Infra.UnitTests.Repositories;

[Collection(nameof(IntegrationTestFixture))]
public class OrderRepositoryTests : BaseTests
{
    private readonly IOrderRepository _orderRepository;

    public OrderRepositoryTests(IntegrationTestFixture fixture)
    {
        _orderRepository = fixture.OrderRepository;
    }

    [Fact]
    public async Task Query_WhenFilterByOrder_ReturnsExpectedOrders()
    {
        // arrange
        var expected = OrderDalModelCreator.Generate();
        await _orderRepository.Add(expected, Token);

        // act
        var query = new OrdersQuery(
            OrderIds: [expected.OrderId],
            RegionIds: [],
            CustomerIds: [],
            Limit: 1,
            Offset: 0);
        var result = await _orderRepository.Query(query, Token);

        // assert
        result.Should().NotBeEmpty();
        result.Should().OnlyContain(o => o.OrderId == expected.OrderId);
    }

    [Fact]
    public async Task Update_WhenFilterByOrder_ReturnsExpectedOrders()
    {
        // arrange
        var order = OrderDalModelCreator.Generate();
        await _orderRepository.Add(order, Token);

        var newOrder = OrderDalModelCreator
            .Generate()
            .WithId(order.OrderId);

        // act
        await _orderRepository.Update(newOrder, Token);

        // assert
        var query = new OrdersQuery(
            OrderIds: [newOrder.OrderId],
            RegionIds: [],
            CustomerIds: [],
            Limit: 1,
            Offset: 0);

        var result = await _orderRepository.Query(query, Token);

        result.Should().NotBeEmpty();

        result.ToList().ForEach(o => o.CreatedAt.Should().BeCloseTo(
            newOrder.CreatedAt,
            FluentAssertionOptions.RequiredDateTimePrecision));

        result.Should().OnlyContain(o => o.Comment == newOrder.Comment);
        result.Should().OnlyContain(o => o.Status == newOrder.Status);
        result.Should().OnlyContain(o => o.RegionId == newOrder.RegionId);
        result.Should().OnlyContain(o => o.CustomerId == newOrder.CustomerId);
    }
}
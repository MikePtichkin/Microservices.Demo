using Moq;
using Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.UnitTests;

public class GetRegionOrdersHandlerTests
{
    private readonly Mock<IOrdersCient> _mockOrdersClient;
    private readonly Mock<ICustomersClient> _mockCustomersClient;
    private readonly GetRegionOrdersHandler _handler;

    public GetRegionOrdersHandlerTests()
    {
        _mockOrdersClient = new Mock<IOrdersCient>();
        _mockCustomersClient = new Mock<ICustomersClient>();
        _handler = new GetRegionOrdersHandler(_mockOrdersClient.Object, _mockCustomersClient.Object);
    }

    [Fact]
    public async Task Handle_OrdersAndCustomersFound_ReturnsSuccessWithOrdersAndCustomerNames()
    {
        // Arrange
        var regionId = 1;
        var limit = 10;
        var offset = 0;
        var regionIds = new long[] { regionId };
        var orders = new Order[]
        {
            new(1, regionId, OrderStatus.New, 1, DateTime.UtcNow, new OrderComment("Test")),
            new(2, regionId, OrderStatus.New, 2, DateTime.UtcNow, new OrderComment("Review"))
        };
        var customerIds = orders.Select(o => o.CustomerId).Distinct().ToArray();
        var customers = new Customer[]
        {
            new(1, regionId, new FullName("John Doe"), DateTime.UtcNow),
            new(2, regionId, new FullName("Jane Doe"), DateTime.UtcNow)
        };

        _mockOrdersClient.Setup(x => x.Query(
            It.IsAny<long[]>(),
            It.IsAny<long[]>(),
            It.Is<long[]>(ids => ids.SequenceEqual(regionIds)),
            It.Is<int>(l => l == limit),
            It.Is<int>(o => o == offset),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(orders);

        _mockCustomersClient.Setup(x => x.Query(
            It.Is<long[]>(ids => ids.SequenceEqual(customerIds)),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(customers);

        var query = new GetRegionOrdersQuery(regionId, limit, offset);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value.OrdersWithCustomerNames);
        Assert.Equal(orders.Length, result.Value.OrdersWithCustomerNames.Count);
        foreach (var orderWithCustomerName in result.Value.OrdersWithCustomerNames)
        {
            var customer = customers.First(c => c.Id == orderWithCustomerName.CustomerId);
            Assert.Equal(customer.FullName.Value, orderWithCustomerName.CustomerName);
        }
    }
}

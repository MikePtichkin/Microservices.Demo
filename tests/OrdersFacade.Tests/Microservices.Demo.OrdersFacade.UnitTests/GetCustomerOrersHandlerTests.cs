using Microservices.Demo.OrdersFacade.Application.Exceptions;
using Microservices.Demo.OrdersFacade.Application.Orders.GetCustomerOrders;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.UnitTests;

public class GetCustomerOrdersHandlerTests
{
    private readonly Mock<IOrdersCient> _mockOrdersClient;
    private readonly Mock<ICustomersClient> _mockCustomersClient;
    private readonly GetCustomerOrdersHandler _handler;

    public GetCustomerOrdersHandlerTests()
    {
        _mockOrdersClient = new Mock<IOrdersCient>();
        _mockCustomersClient = new Mock<ICustomersClient>();
        _handler = new GetCustomerOrdersHandler(_mockOrdersClient.Object, _mockCustomersClient.Object);
    }

    [Fact]
    public async Task Handle_NoCustomersFound_ThrowsCustomerNotFoundException()
    {
        // Arrange
        long customerId = 1;
        var query = new GetCustomerOrdersQuery(customerId, 10, 0);

        _mockCustomersClient.Setup(x => x.Query(
            It.Is<long[]>(ids => ids.SequenceEqual(new long[] { customerId })),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync([]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomerNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Equal($"Customer with id {customerId} not found", exception.Message);
    }

    [Fact]
    public async Task Handle_CustomersFound_ReturnsSuccessWithOrders()
    {
        // Arrange
        long customerId = 1;
        var query = new GetCustomerOrdersQuery(customerId, 10, 0);
        var customers = new Customer[]
        {
            new(customerId, 1, new FullName("John Doe"), DateTime.UtcNow)
        };
        var orders = new Order[]
        {
            new(1, 1, OrderStatus.New, customerId, DateTime.UtcNow, new OrderComment("Test"))
        };

        _mockCustomersClient.Setup(x => x.Query(
            It.Is<long[]>(ids => ids.SequenceEqual(new long[] { customerId })),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(customers);

        _mockOrdersClient.Setup(x => x.Query(
            It.Is<long[]>(cIds => cIds.SequenceEqual(new long[] { customerId })),
            It.Is<long[]>(oIds => !oIds.Any()),  // Проверка, что массив идентификаторов заказов пуст
            It.Is<long[]>(rIds => !rIds.Any()),  // Проверка, что массив идентификаторов регионов пуст
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(customers[0], result.Value.Customer);
        Assert.Equal(orders.Length, result.Value.Orders.Count);  // Сравнение количества заказов
        Assert.Contains(orders[0], result.Value.Orders);  // Проверка наличия конкретного заказа в результате
    }
}

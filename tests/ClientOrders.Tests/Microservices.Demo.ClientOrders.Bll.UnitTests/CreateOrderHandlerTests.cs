using AutoFixture;
using Moq;
using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;
using Microservices.Demo.ClientOrders.Customers;
using Microservices.Demo.ClientOrders.Domain.Common;
using Microservices.Demo.ClientOrders.Domain.Customers;
using Microservices.Demo.ClientOrders.Domain.Orders;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;

namespace Microservices.Demo.ClientOrders.Bll.UnitTests;

public sealed class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_Should_CreateOrder_PublishToKafka_WhenInvoked()
    {
        // Arrange
        var fixture = new Fixture();
        var faker = new Bogus.Faker();
        var random = new Random();

        var regionId = random.Next(1, 4);
        var fullName = new FullName($"{faker.Person.FirstName} {faker.Person.LastName}");
        var createdAt = fixture.Create<TimeStamp>();
        var customerId = fixture.Create<long>();
        var customer = new Customer(customerId, regionId, fullName, createdAt);

        var stockItems = fixture.CreateMany<StockItem>().ToArray();
        var command = new CreateOrderCommand(customerId, stockItems);

        var mockedCustomerClient = new Mock<ICustomerClient>();
        mockedCustomerClient
            .Setup(x => x.Query(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var mockedOrdersRepository = new Mock<IOrdersRepository>();
        var mockedOrderInputEventPublisher = new Mock<IOrdersInputEventPublisher>();

        var handler = new CreateOrderHandler(
            mockedCustomerClient.Object,
            mockedOrdersRepository.Object,
            mockedOrderInputEventPublisher.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockedCustomerClient.Verify(x => x.Query(
            It.IsAny<long>(),
            It.IsAny<CancellationToken>()), Times.Once);

        mockedOrdersRepository.Verify(x => x.Add(
            It.IsAny<Order>(),
            It.IsAny<CancellationToken>()), Times.Once);

        mockedOrderInputEventPublisher.Verify(x => x.PublishToKafka(
            It.IsAny<OrdersInputMessage>(),
            It.IsAny<CancellationToken>()), Times.Once);

        mockedCustomerClient.VerifyNoOtherCalls();
        mockedOrdersRepository.VerifyNoOtherCalls();
        mockedOrderInputEventPublisher.VerifyNoOtherCalls();
    }
}

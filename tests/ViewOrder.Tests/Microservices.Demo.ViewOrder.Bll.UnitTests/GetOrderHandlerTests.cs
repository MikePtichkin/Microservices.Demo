using FluentAssertions;
using Moq;
using Microservices.Demo.ViewOrder.Bll.Exceptions;
using Microservices.Demo.ViewOrder.Bll.Orders.Features.OrderOutputEventMessage;
using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using Microservices.Demo.ViewOrder.Bll.UnitTests.Creators;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Bll.UnitTests;

public class GetOrderHandlerTests
{
    private readonly Mocks _mocks;
    private readonly OrderOutputEventMessageHandler _handler;

    public GetOrderHandlerTests()
    {
        _mocks = new Mocks();
        _handler = new OrderOutputEventMessageHandler(
            _mocks.OrderClientMock.Object,
            _mocks.OrderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidOrder_AddsOrderToRepository()
    {
        // arrange
        var bllOrder = OrderClientModelCreator.Generate();
        _mocks.OrderClientMock
            .Setup(x => x.Query(bllOrder.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bllOrder);

        // Act
        await _handler.Handle(new OrderOutputEventMessage(bllOrder.OrderId), CancellationToken.None);

        // Assert
        _mocks.OrderClientMock
            .Verify(x => x.Query(bllOrder.OrderId, It.IsAny<CancellationToken>()), Times.Once);
        _mocks.OrderClientMock.VerifyNoOtherCalls();

        _mocks.OrderRepositoryMock
            .Verify(x => x.Add(
                It.IsAny<OrderDalModel>(),
                It.IsAny<CancellationToken>()), Times.Once);
        _mocks.OrderRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_InvalidOrder_ThrowsOrderNotFoundException()
    {
        // arrange
        long nonExistentOrderId = 9999;
        _mocks.OrderClientMock
            .Setup(x => x.Query(nonExistentOrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderClientModel)null!);
        var query = new OrderOutputEventMessage(nonExistentOrderId);

        // act
        var act = () => _handler.Handle(query, CancellationToken.None);

        // assert
        await act.Should().ThrowExactlyAsync<OrderNotFoundException>();
    }
}

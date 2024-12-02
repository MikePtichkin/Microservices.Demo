using Moq;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;

namespace Microservices.Demo.ViewOrder.Bll.UnitTests;

public class Mocks
{
    public readonly Mock<IOrderRepository> OrderRepositoryMock = new();
    public readonly Mock<IOrderClient> OrderClientMock = new();
}

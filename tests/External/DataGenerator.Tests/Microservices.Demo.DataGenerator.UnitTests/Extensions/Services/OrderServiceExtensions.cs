using Moq;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.UnitTests.Extensions.Services;

public static class OrderServiceExtensions
{
    public static Mock<IOrderService> SetupCreateQueue(
        this Mock<IOrderService> mock,
        IReadOnlyList<Customer> customers,
        Queue<Order> order)
    {
        mock.Setup(
                service => service.Create(customers))
            .Returns(order.Dequeue);

        return mock;
    }

    public static Mock<IOrderService> VerifyCreate(
        this Mock<IOrderService> mock,
        IReadOnlyList<Customer> customers,
        int times = 1)
    {
        mock.Verify(
            service => service.Create(customers),
            Times.Exactly(times));

        return mock;
    }
}

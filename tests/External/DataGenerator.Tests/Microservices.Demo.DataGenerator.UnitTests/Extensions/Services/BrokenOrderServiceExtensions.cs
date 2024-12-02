using Moq;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.UnitTests.Extensions.Services;

public static class BrokenOrderServiceExtensions
{
    public static Mock<IBrokenOrderService> SetupBreakOrder(
        this Mock<IBrokenOrderService> mock,
        Order order,
        Order brokenOrder)
    {
        mock.Setup(
                service => service.BreakOrder(order))
            .Returns(brokenOrder);

        return mock;
    }

    public static Mock<IBrokenOrderService> VerifyBreakOrder(
        this Mock<IBrokenOrderService> mock,
        Order order,
        int times = 1)
    {
        mock.Verify(
                service => service.BreakOrder(order),
                Times.Exactly(times));

        return mock;
    }
}

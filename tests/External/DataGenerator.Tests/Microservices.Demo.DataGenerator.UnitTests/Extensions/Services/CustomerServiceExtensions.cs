using Moq;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.UnitTests.Extensions.Services;

public static class CustomerServiceExtensions
{
    public static Mock<ICustomerService> SetupCreate(
        this Mock<ICustomerService> mock,
        int count,
        IReadOnlyList<Customer> customers)
    {
        mock.Setup(
                service => service.Create(count))
            .Returns(customers);

        return mock;
    }

    public static Mock<ICustomerService> VerifyCreate(
        this Mock<ICustomerService> mock,
        int count,
        int times = 1)
    {
        mock.Verify(
            service => service.Create(count),
            Times.Exactly(times));

        return mock;
    }
}

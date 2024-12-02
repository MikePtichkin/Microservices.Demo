using Moq;

using Microservices.Demo.CustomerService.Domain;
using Microservices.Demo.CustomerService.DomainServices.CreateCustomer;
using Microservices.Demo.CustomerService.Repositories;
using Microservices.Demo.CustomerService.Repositories.Exceptions;

namespace Microservices.Demo.CustomerService.UnitTests.DomainServices;

public sealed class CreateCustomerCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenRegionDoesntExist_ReturnsError()
    {
        var customerRepositoryStub = new Mock<ICustomerRepository>();
        var regionRepositoryStub = new Mock<IRegionRepository>();
        var exception = new RegionNotFoundException("Not found");
        regionRepositoryStub
            .Setup(r => r.Get(4, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        var handler = new CreateCustomerCommandHandler(customerRepositoryStub.Object, regionRepositoryStub.Object);
        var response = await handler.Handle(new CreateCustomerCommandRequest("Test User", 4), CancellationToken.None);

        Assert.False(response.Successful);
        Assert.Equal(exception, response.Exception);
    }

    [Fact]
    public async Task Handle_WhenCustomerAlreadyExists_ReturnsError()
    {
        var customerRepositoryStub = new Mock<ICustomerRepository>();
        var regionRepositoryStub = new Mock<IRegionRepository>();
        regionRepositoryStub
            .Setup(r => r.Get(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Region(1, "Москва"));
        var exception = new CustomerAlreadyExistsException("Already Exists");
        customerRepositoryStub
            .Setup(r => r.CreateCustomer(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        var handler = new CreateCustomerCommandHandler(customerRepositoryStub.Object, regionRepositoryStub.Object);
        var response = await handler.Handle(new CreateCustomerCommandRequest("Test User", 1), CancellationToken.None);

        Assert.False(response.Successful);
        Assert.Equal(exception, response.Exception);
    }

    [Fact]
    public async Task Handle_WhenRequestIsCorrect_ReturnsId()
    {
        var customerRepositoryStub = new Mock<ICustomerRepository>();
        var regionRepositoryStub = new Mock<IRegionRepository>();
        regionRepositoryStub
            .Setup(r => r.Get(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Region(1, "Москва"));
        customerRepositoryStub
            .Setup(r => r.CreateCustomer(It.IsAny<string>(), 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(123);
        var handler = new CreateCustomerCommandHandler(customerRepositoryStub.Object, regionRepositoryStub.Object);
        var response = await handler.Handle(new CreateCustomerCommandRequest("Test User", 1), CancellationToken.None);

        Assert.True(response.Successful);
        Assert.Equal(123, response.CustomerId);
    }
}
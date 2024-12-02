using Moq;

using Microservices.Demo.CustomerService.Domain;
using Microservices.Demo.CustomerService.DomainServices.GetCustomers;
using Microservices.Demo.CustomerService.Repositories;
using Microservices.Demo.CustomerService.Repositories.Exceptions;

namespace Microservices.Demo.CustomerService.UnitTests.DomainServices;

public sealed class GetCustomersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Always_ReturnsFromRepository()
    {
        var customerRepositoryStub = new Mock<ICustomerRepository>();
        var exception = new RegionNotFoundException("Not found");
        var customer = new Customer
        {
            Id = 123,
            Region = new Region(1, "Москва"),
            FullName = "Test User",
            CreatedAt = DateTime.UtcNow
        };
        customerRepositoryStub
            .Setup(r => r.GetCustomers(new long[] {123}, Array.Empty<long>(), 10, 0, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerQueryModel(1, [customer]));
        var handler = new GetCustomersQueryHandler(customerRepositoryStub.Object);
        var response = await handler.Handle(new GetCustomersQueryRequest([123], Array.Empty<long>(), 10, 0), CancellationToken.None);

        Assert.Equal(1, response.TotalCount);
        Assert.Equal(customer, response.Customers.Single());
    }
}
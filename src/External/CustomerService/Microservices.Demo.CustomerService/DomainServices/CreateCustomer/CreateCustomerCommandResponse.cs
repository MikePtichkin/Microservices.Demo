using MediatR;

using Microservices.Demo.CustomerService.Domain;

namespace Microservices.Demo.CustomerService.DomainServices.CreateCustomer;

public sealed class CreateCustomerCommandResponse
{
    public CreateCustomerCommandResponse(long customerId)
    {
        Successful = true;
        CustomerId = customerId;
    }

    public CreateCustomerCommandResponse(Exception exception)
    {
        Successful = false;
        Exception = exception;
    }

    public bool Successful { get; }
    public long? CustomerId { get; }
    public Exception? Exception { get; }
}
using Microservices.Demo.OrdersFacade.Domain.Abstraction;

namespace Microservices.Demo.OrdersFacade.Domain.Customers;

public static class CustomerErrors
{
    public static Error NotFound = new(
        "Customer.NotFound",
        "Customer with provided id was not found");
}

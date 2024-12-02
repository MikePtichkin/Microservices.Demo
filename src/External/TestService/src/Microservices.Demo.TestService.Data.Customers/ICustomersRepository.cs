namespace Microservices.Demo.TestService.Data.Customers;

public interface ICustomersRepository
{
    Task<Customer?> GetCustomerByIdAsync(long customerId, CancellationToken cancellationToken);
}

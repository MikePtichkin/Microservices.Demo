using Microservices.Demo.DataGenerator.Bll.Models;

namespace Microservices.Demo.DataGenerator.Bll.ProviderContracts;

public interface ICustomerProvider
{
    Task<long?> CreateCustomer(Customer customer, CancellationToken token);
}

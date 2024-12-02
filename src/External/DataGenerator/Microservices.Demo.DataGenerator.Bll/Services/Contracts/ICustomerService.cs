using Microservices.Demo.DataGenerator.Bll.Models;

namespace Microservices.Demo.DataGenerator.Bll.Services.Contracts;

public interface ICustomerService
{
    IReadOnlyList<Customer> Create(int count);
}

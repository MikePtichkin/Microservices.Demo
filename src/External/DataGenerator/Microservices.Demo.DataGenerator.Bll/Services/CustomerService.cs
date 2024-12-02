using Microservices.Demo.DataGenerator.Bll.Creators;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.Bll.Services;

public class CustomerService : ICustomerService
{
    public IReadOnlyList<Customer> Create(int count) => CustomerCreator.Create(count);
}

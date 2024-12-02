using Microservices.Demo.ClientOrders.Bll.Exceptions;
using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Domain.Customers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infra.Orders.Decorators;

internal sealed class CustomerClientCachedDecorator
    : ICustomerClient
{
    private readonly ICustomerClient _innerCustomerClient;
    private readonly ICustomerCacheRepository _customerCacheRepository;

    public CustomerClientCachedDecorator(
        ICustomerClient innerCustomersClient,
        ICustomerCacheRepository cacheRepository)
    {
        _innerCustomerClient = innerCustomersClient;
        _customerCacheRepository = cacheRepository;
    }

    public async Task<Customer?> Query(
        long customerId,
        CancellationToken token)
    {
        var customer = await _customerCacheRepository.GetCustomer(
            customerId,
            token);

        if (customer is null)
        {
            customer = await _innerCustomerClient.Query(
                customerId,
                token) ?? throw new CustomerNotFoundException(customerId);

            await _customerCacheRepository.InsertCustomer(
                customer,
                token);
        }

        return customer;
    }
}

using Microservices.Demo.ClientOrders.Customers;
using Microservices.Demo.ClientOrders.Domain.Customers;
using Microservices.Demo.CustomerService;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microservices.Demo.CustomerService.CustomerService;

namespace Microservices.Demo.ClientOrders.Infra.Clients.Grpc;

internal sealed class CustomersGrpcClient : ICustomerClient
{
    private readonly CustomerServiceClient _customersClient;

    public CustomersGrpcClient(CustomerServiceClient customersClient)
    {
        _customersClient = customersClient;
    }

    public async Task<Customer?> Query(long customerId, CancellationToken token)
    {
        var request = new V1QueryCustomersRequest()
        {
            CustomerIds = { customerId },
            Limit = 1,
            Offset = 0
        };

        using var call = _customersClient.V1QueryCustomers(request, cancellationToken: token);
        var responseStream = call.ResponseStream;
        var customers = new List<Customer>();

        while (await responseStream.MoveNext(token))
        {
            var response = responseStream.Current;

            var customerDomain = new Customer(
                id: response.Customer.CustomerId,
                regionId: response.Customer.Region.Id,
                fullName: new FullName(response.Customer.FullName),
                createdAt: response.Customer.CreatedAt.ToDateTimeOffset());

            customers.Add(customerDomain);
        }

        return customers.FirstOrDefault();
    }
}

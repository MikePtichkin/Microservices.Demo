using Microservices.Demo.CustomerService;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Microservices.Demo.CustomerService.CustomerService;

namespace Microservices.Demo.OrdersFacade.Infra.Clients.Grpc;

internal sealed class CustomersGrpcClient : ICustomersClient
{
    private readonly CustomerServiceClient _customersClient;

    public CustomersGrpcClient(CustomerServiceClient customersClient)
    {
        _customersClient = customersClient;
    }

    public async Task<Customer[]> Query(
        long[] customerIds,
        int limit,
        int offset,
        CancellationToken token)
    {
        var request = new V1QueryCustomersRequest()
        {
            CustomerIds = { customerIds },
            Limit = limit,
            Offset = offset
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
                createdAt: response.Customer.CreatedAt.ToDateTime());

            customers.Add(customerDomain);
        }

        return [.. customers];
    }
}

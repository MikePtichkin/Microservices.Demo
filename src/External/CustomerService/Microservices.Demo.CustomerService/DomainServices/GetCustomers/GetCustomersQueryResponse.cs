using MediatR;

using Microservices.Demo.CustomerService.Domain;

namespace Microservices.Demo.CustomerService.DomainServices.GetCustomers;

public sealed record GetCustomersQueryResponse(Customer[] Customers, long TotalCount)
{
}
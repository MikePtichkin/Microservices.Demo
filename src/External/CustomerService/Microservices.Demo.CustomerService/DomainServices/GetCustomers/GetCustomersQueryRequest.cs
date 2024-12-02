using MediatR;

namespace Microservices.Demo.CustomerService.DomainServices.GetCustomers;

public sealed record GetCustomersQueryRequest(long[] CustomerIds, long[] RegionIds, int Limit,  int Offset) : IRequest<GetCustomersQueryResponse>;
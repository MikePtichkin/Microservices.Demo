using MediatR;

namespace Microservices.Demo.CustomerService.DomainServices.CreateCustomer;

public sealed record CreateCustomerCommandRequest(string FullName, long RegionId) : IRequest<CreateCustomerCommandResponse>;
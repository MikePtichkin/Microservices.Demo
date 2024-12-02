using MediatR;

namespace Microservices.Demo.TestService.Domain.Actions.ListMismatches;

public class ListMismatchesQuery : IRequest<MismatchStatistics>
{
}

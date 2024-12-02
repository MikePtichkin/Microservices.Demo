using MediatR;

namespace Microservices.Demo.TestService.Domain.Actions.EnableMismatchFeature;

public class EnableMismatchFeatureCommand : IRequest
{
    public TimeSpan? Duration { get; set; }
}

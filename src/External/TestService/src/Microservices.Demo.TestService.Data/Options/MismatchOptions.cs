namespace Microservices.Demo.TestService.Data;

public class MismatchOptions
{
    public required int SamplingDurationSec { get; set; }

    public TimeSpan SamplingDuration => TimeSpan.FromSeconds(SamplingDurationSec);
}

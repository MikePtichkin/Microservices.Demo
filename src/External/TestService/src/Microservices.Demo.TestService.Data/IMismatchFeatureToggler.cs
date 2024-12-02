namespace Microservices.Demo.TestService.Data;

public interface IMismatchFeatureToggler
{
    void Enable(TimeSpan duration);

    void Disable();
}

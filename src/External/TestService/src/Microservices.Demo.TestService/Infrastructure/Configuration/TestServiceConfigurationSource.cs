namespace Microservices.Demo.TestService;

public class TestServiceConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) => new TestServiceEnvironmentConfigurationProvider();
}

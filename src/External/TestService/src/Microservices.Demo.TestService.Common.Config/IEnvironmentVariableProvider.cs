namespace Microservices.Demo.TestService.Common.Config;

public interface IEnvironmentVariableProvider
{
    string? GetValue(string key);
}

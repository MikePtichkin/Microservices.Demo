namespace Microservices.Demo.CustomerService.Repositories.Exceptions;

public sealed class RegionNotFoundException(string message) : Exception(message);
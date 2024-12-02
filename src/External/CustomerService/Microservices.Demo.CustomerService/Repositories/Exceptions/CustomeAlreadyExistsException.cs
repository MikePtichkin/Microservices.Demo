namespace Microservices.Demo.CustomerService.Repositories.Exceptions;

public sealed class CustomerAlreadyExistsException(string message) : Exception(message);
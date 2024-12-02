namespace Microservices.Demo.DataGenerator.Infra.Exceptions;

public class InvalidKafkaConfigurationException : Exception
{
    public InvalidKafkaConfigurationException(string message) : base(message) { }
}

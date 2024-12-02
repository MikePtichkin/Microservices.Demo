namespace Microservices.Demo.DataGenerator.Infra.Kafka.Interfaces;

public interface IKafkaProducer
{
    Task SendMessage(
        long key,
        string value,
        string topic,
        CancellationToken token);
}

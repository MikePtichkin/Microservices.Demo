namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public interface IKafkaConsumerFactory
{
    IKafkaConsumer CreateConsumer(ConsumerConfiguration configuration);
}

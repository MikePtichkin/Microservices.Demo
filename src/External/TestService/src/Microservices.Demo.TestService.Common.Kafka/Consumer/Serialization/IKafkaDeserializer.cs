using Confluent.Kafka;

namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public interface IKafkaDeserializer<T>
    where T : class
{
    KafkaMessagePart<T> Deserialize(byte[] data, SerializationContext context);
}

using Confluent.Kafka;

namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public interface IMessageDeserializer<TKey, TValue>
    where TKey : class
    where TValue : class
{
    KafkaMessage<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>> DeserializeMessage(ConsumeResult<byte[], byte[]> consumed);
}

using Confluent.Kafka;

namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public interface IKafkaConsumer : IDisposable
{
    ConsumeResult<byte[], byte[]>? Consume(CancellationToken cancellationToken);

    void StoreOffset(ConsumeResult<byte[], byte[]> consumed);

    void Subscribe(string topic);
}

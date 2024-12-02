using Confluent.Kafka;

namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public class ConsumerConfiguration : ConsumerConfig
{
    public required string Topic { get; init; }
}

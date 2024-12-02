using Confluent.Kafka;

namespace Microservices.Demo.OrderService.Kafka.Settings;

public class ProducerSettings
{
    public Acks Acks { get; set; } = Acks.Leader;

    public bool EnableIdempotence { get; set; } = false;
}
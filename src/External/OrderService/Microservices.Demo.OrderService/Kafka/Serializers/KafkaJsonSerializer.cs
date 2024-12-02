using System.Text.Json;
using Confluent.Kafka;

namespace Microservices.Demo.OrderService.Kafka.Serializers;

public class KafkaJsonSerializer<TMessage> : IDeserializer<TMessage>
{
    public TMessage Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
    {
        return JsonSerializer.Deserialize<TMessage>(data)!;
    }
}
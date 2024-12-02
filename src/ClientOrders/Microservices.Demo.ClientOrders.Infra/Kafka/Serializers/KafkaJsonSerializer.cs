using Confluent.Kafka;
using System;
using System.Text.Json;

namespace Microservices.Demo.ClientOrders.Infra.Kafka.Serializers;

internal sealed class KafkaJsonSerializer<TMessage>
    : IDeserializer<TMessage>, ISerializer<TMessage>
{
    public TMessage Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
    {
        return JsonSerializer.Deserialize<TMessage>(data)!;
    }

    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
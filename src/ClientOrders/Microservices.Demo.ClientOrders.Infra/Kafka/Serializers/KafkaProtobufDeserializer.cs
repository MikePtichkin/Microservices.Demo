using Confluent.Kafka;
using Google.Protobuf;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Kafka.Serializers;

internal sealed class KafkaProtobufDeserializer<TMessage>
    : IDeserializer<TMessage> where TMessage : IMessage<TMessage>, new()
{
    public TMessage Deserialize(
        ReadOnlySpan<byte> data,
        bool isNull,
        SerializationContext context)
    {
        var parser = new MessageParser<TMessage>(() => new TMessage());
        return parser.ParseFrom(data.ToArray());
    }
}

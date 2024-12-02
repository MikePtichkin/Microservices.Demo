﻿using System.Text;
using Confluent.Kafka;

namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public class KafkaUtf8StringDeserializer : IKafkaDeserializer<string>
{
    public KafkaMessagePart<string> Deserialize(byte[] data, SerializationContext context)
    {
        return new KafkaMessagePart<string>
        {
            RawValue = data.ToArray(),
            Payload = Encoding.UTF8.GetString(data),
        };
    }
}

using System.Text.Json;
using Microsoft.Extensions.Options;
using Microservices.Demo.DataGenerator.Bll.ProviderContracts;
using Microservices.Demo.DataGenerator.Infra.Exceptions;
using Microservices.Demo.DataGenerator.Infra.Kafka;
using Microservices.Demo.DataGenerator.Infra.Kafka.Interfaces;
using Microservices.Demo.DataGenerator.Messages;

namespace Microservices.Demo.DataGenerator.Infra.Providers;

public class KafkaProvider : IKafkaProvider
{
    private readonly IKafkaProducer _producer;
    private readonly string _ordersInputTopic;

    public KafkaProvider(
        IKafkaProducer producer,
        IOptions<KafkaTopics> kafkaTopics)
    {
        _producer = producer;

        _ordersInputTopic = string.IsNullOrWhiteSpace(kafkaTopics.Value.OrdersInput)
            ? throw new InvalidKafkaConfigurationException($"Topic name {nameof(kafkaTopics.Value.OrdersInput)} is empty")
            : kafkaTopics.Value.OrdersInput;
    }

    public Task Publish(OrderInputMessage message, CancellationToken token)
    {
        var key = message.CustomerId;
        var value = JsonSerializer.Serialize(message);

        return _producer.SendMessage(
            key,
            value,
            _ordersInputTopic,
            token);
    }
}

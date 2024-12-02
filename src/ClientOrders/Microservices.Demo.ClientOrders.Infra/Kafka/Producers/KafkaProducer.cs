using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infra.Kafka.Producers;

internal sealed class KafkaProducer : IDisposable, IKafkaProducer
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<long, string> _producer;

    public KafkaProducer(
        ILogger<KafkaProducer> logger,
        string connectionString)
    {
        _logger = logger;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = connectionString,
            Acks = Acks.Leader,
            EnableIdempotence = false,
            LingerMs = 50,
            Partitioner = Partitioner.ConsistentRandom
        };

        _producer = new ProducerBuilder<long, string>(producerConfig).Build();
    }

    public async Task SendMessage(
        long key,
        string value,
        string topic,
        CancellationToken token)
    {
        try
        {
            var message = new Message<long, string>
            {
                Key = key,
                Value = value,
            };

            await _producer.ProduceAsync(topic, message, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in producing to {Topic}", topic);
            throw;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _producer.Flush();
        _producer.Dispose();
    }
}

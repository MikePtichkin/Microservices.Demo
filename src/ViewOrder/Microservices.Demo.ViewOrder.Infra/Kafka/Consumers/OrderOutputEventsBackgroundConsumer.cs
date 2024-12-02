using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microservices.Demo.ViewOrder.Bll.Orders.Features.OrderOutputEventMessage;
using Microservices.Demo.ViewOrder.Infra.Kafka.Serializers;
using Microservices.Demo.ViewOrder.Infra.Kafka.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Infra.Kafka.Consumers;

internal sealed class OrderOutputEventsBackgroundConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<OrderOutputEventsBackgroundConsumer> _logger;

    private readonly ConsumerConfig _consumerConfig;
    private readonly string _topic;
    private readonly TimeSpan _timeoutForRetry;

    public OrderOutputEventsBackgroundConsumer(
        IServiceProvider serviceProvider,
        KafkaSettings kafkaSettings,
        ConsumerSettings consumerSettings)
    {
        _serviceProvider = serviceProvider;

        _logger = _serviceProvider.GetRequiredService<ILogger<OrderOutputEventsBackgroundConsumer>>();

        _topic = consumerSettings.Topic;
        _timeoutForRetry = TimeSpan.FromSeconds(kafkaSettings.TimeoutForRetryInSeconds);

        _consumerConfig = new ConsumerConfig
        {
            GroupId = kafkaSettings.GroupId,
            BootstrapServers = kafkaSettings.BootstrapServers,
            EnableAutoCommit = consumerSettings.AutoCommit
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(OrderOutputEventsBackgroundConsumer)} | Started");

        await Task.Yield();

        var consumer = new ConsumerBuilder<string, OrderService.Proto.Messages.OrderOutputEventMessage>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(new KafkaProtobufDeserializer<OrderService.Proto.Messages.OrderOutputEventMessage>())
            .Build();

        consumer.Subscribe(_topic);
        _logger.LogInformation("Successfull subscription to {Topic}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            int? partition = null;
            long? offset = null;

            try
            {
                var message = consumer.Consume(stoppingToken);

                partition = message.Partition.Value;
                offset = message.Offset.Value;

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

                var orderId = message.Message.Value.OrderId;
                var bllMessage = new OrderOutputEventMessage(orderId);
                await mediator.Send(bllMessage, stoppingToken);

                consumer.Commit();
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(
                    ex,
                    "{ConsumerName} | Consume error: topic {Topic}, partition {Partition}, offset {Offset}",
                    nameof(OrderOutputEventsBackgroundConsumer),
                    ex.ConsumerRecord?.Topic,
                    ex.ConsumerRecord?.Partition,
                    ex.ConsumerRecord?.Offset);

                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "{ConsumerName} | Unexpexted consume error: topic {Topic}, partition {Partition}, offset {Offset}",
                    nameof(OrderOutputEventsBackgroundConsumer),
                    _topic,
                    partition,
                    offset);

                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
        }
    }
}

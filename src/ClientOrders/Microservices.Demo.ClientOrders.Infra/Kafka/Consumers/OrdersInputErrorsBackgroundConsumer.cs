using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;
using Microservices.Demo.ClientOrders.Domain.Orders;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;
using Microservices.Demo.ClientOrders.Infra.Kafka.Serializers;
using Microservices.Demo.ClientOrders.Infra.Kafka.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infra.Kafka.Consumers;

public class OrdersInputErrorsBackgroundConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrdersInputErrorsBackgroundConsumer> _logger;
    private readonly ConsumerConfig _consumerConfig;
    private readonly string _topic;
    private readonly TimeSpan _timeoutForRetry;

    public OrdersInputErrorsBackgroundConsumer(
        IServiceProvider serviceProvider,
        KafkaSettings kafkaSettings,
        ConsumerSettings consumerSettings)
    {
        _serviceProvider = serviceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<OrdersInputErrorsBackgroundConsumer>>();
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
        _logger.LogInformation("OrdersInputErrorsBackgroundConsumer | Started");

        await Task.Yield();

        var consumer = new ConsumerBuilder<string, OrdersInputErrorsMessage>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(new KafkaJsonSerializer<OrdersInputErrorsMessage>())
            .Build();

        consumer.Subscribe(_topic);
        _logger.LogInformation("Successfull subscription to {Topic}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = consumer.Consume(stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

                var comment = message.Message.Value.InputOrder.Comment;
                var customerId = message.Message.Value.InputOrder.CustomerId;

                if (comment is not null && 
                    OrderIdParser.TryParseOrderIdFromComment(comment, out var id, out var guid) &&
                    await orderRepository.Get(id, stoppingToken) is Order domainOrder &&
                    domainOrder.CustomerId == customerId &&
                    domainOrder.Comment == guid.ToString())
                {
                    domainOrder.Cancel();

                    var error = message.Message.Value.ErrorReason.ToString();
                    domainOrder.SetError(error);

                    await orderRepository.Update(domainOrder, stoppingToken);
                }

                consumer.Commit();
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex,
                    "{ConsumerError} | Consume error: {Topic}, partition: {Partition}, offset: {Offset}",
                    ex.ConsumerRecord?.Topic,
                    ex.ConsumerRecord?.Partition,
                    ex.ConsumerRecord?.Offset,
                    nameof(OrdersInputErrorsBackgroundConsumer));

                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "{ConsumerName} | Error during kafka consume",
                    nameof(OrdersInputErrorsBackgroundConsumer));

                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
        }
    }
}

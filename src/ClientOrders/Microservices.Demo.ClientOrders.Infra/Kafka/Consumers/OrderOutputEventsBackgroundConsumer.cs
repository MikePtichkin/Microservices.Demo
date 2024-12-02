using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Domain.Orders;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;
using Microservices.Demo.ClientOrders.Infra.Extensions;
using Microservices.Demo.ClientOrders.Infra.Kafka.Serializers;
using Microservices.Demo.ClientOrders.Infra.Kafka.Settings;
using Microservices.Demo.OrderService.Proto.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Order = Microservices.Demo.ClientOrders.Bll.Models.Order;
using OrderDomain = Microservices.Demo.ClientOrders.Domain.Orders.Order;


namespace Microservices.Demo.ClientOrders.Infra.Kafka.Consumers;

public class OrderOutputEventsBackgroundConsumer : BackgroundService
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
        _logger.LogInformation("OrderOutputEventBackgroundConsumer | Started");

        await Task.Yield();

        var consumer = new ConsumerBuilder<string, OrderService.Proto.Messages.OrderOutputEventMessage>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(new KafkaProtobufDeserializer<OrderService.Proto.Messages.OrderOutputEventMessage>())
            .Build();

        consumer.Subscribe(_topic);
        _logger.LogInformation("Successfull subscription to {Topic}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = consumer.Consume(stoppingToken);

                await UpdateDomainOrder(message, stoppingToken);

                consumer.Commit();
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(
                    ex,
                    "{ConsumerName} | Consume error: topic {Topic}, partition: {Partition}, offset: {Offset}",
                    ex.ConsumerRecord?.Topic,
                    ex.ConsumerRecord?.Partition,
                    ex.ConsumerRecord?.Offset,
                    nameof(OrderOutputEventsBackgroundConsumer));

                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "{ConsumerName} | Unexpexted error during kafka consume",
                    nameof(OrderOutputEventsBackgroundConsumer));

                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
        }
    }

    private async Task UpdateDomainOrder(ConsumeResult<string, OrderOutputEventMessage> message, CancellationToken stoppingToken)
    {
        /*
         * В случае получения сообщения из топика order_output_events
         * в статусе CREATED необходимо сходить в ручку V1QueryOrders,
         * чтобы по полю comment и customer_id сопоставить с запросом.
         * Если сообщение пришло в другом статусе, то необходимо обновить
         * данные, если такой заказ был сделан через ваш сервис.
         */

        using var scope = _serviceProvider.CreateScope();
        var orderClient = scope.ServiceProvider.GetRequiredService<IOrderClient>();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

        var orderId = message.Message.Value.OrderId;
        var bllOrder = await orderClient.Query(orderId, stoppingToken);

        var domainOrder = await TryGetDomainOrder(bllOrder, stoppingToken);

        if (domainOrder is not null)
        {
            domainOrder.Confirm(orderId);

            if (message.Message.Value.EventType is not OutputEventType.Created)
            {
                domainOrder.SetStatus(bllOrder!.Status.ToDomain());
            }

            await orderRepository.Update(domainOrder, stoppingToken);
        }

        async Task<OrderDomain?> TryGetDomainOrder(Order? bllOrder, CancellationToken stoppingToken)
        {
            if (bllOrder is null || bllOrder.Comment is null)
            {
                // Не нашли заказ по внешнему id или у заказа не заолнен комментарий
                return null;
            }

            if (!OrderIdParser.TryParseOrderIdFromComment(bllOrder.Comment, out var id, out var guid))
            {
                return null;
            }

            var domainOrder = await orderRepository.Get(id, stoppingToken);
            if (domainOrder is null)
            {
                return null;
            }


            if (domainOrder.CustomerId == bllOrder.CustomerId &&
                domainOrder.Comment == guid.ToString())
            {
                return domainOrder;
            }

            return null;
        }
    }
}

using Confluent.Kafka;
using Microservices.Demo.OrderService.Bll.Models;
using Microservices.Demo.OrderService.Bll.Services.Interfaces;
using Microservices.Demo.OrderService.Kafka.Extensions;
using Microservices.Demo.OrderService.Kafka.Messages;
using Microservices.Demo.OrderService.Kafka.Serializers;
using Microservices.Demo.OrderService.Kafka.Settings;
using InputOrder = Microservices.Demo.OrderService.Bll.Models.InputOrder;

namespace Microservices.Demo.OrderService.Kafka.Consumers;

public class InputOrderBackgroundConsumer : BackgroundService
{
    private readonly ILogger<InputOrderBackgroundConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConsumerConfig _consumerConfig;
    private readonly TimeSpan _timeoutForRetry;
    private readonly string _topic;

    public InputOrderBackgroundConsumer(
        IServiceProvider serviceProvider,
        KafkaSettings kafkaSettings,
        ConsumerSettings consumerSettings)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<InputOrderBackgroundConsumer>>();
        _serviceProvider = serviceProvider;
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
        _logger.LogInformation("InputOrderBackgroundConsumer | Started");

        await Task.Yield();
        
        var consumer = new ConsumerBuilder<long, InputOrderMessage>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Int64)
            .SetValueDeserializer(new KafkaJsonSerializer<InputOrderMessage>())
            .Build();
        
        consumer.Subscribe(_topic);
        _logger.LogInformation("Success subscribe to {Topic}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = consumer.Consume(stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

                await service.CreateOrder(message.Message.Value.ToBll(), stoppingToken);

                consumer.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "InputOrderBackgroundConsumer | Error during kafka consume");
                await Task.Delay(_timeoutForRetry, stoppingToken);
            }
        }
        
        consumer.Unsubscribe();
        _logger.LogInformation("Success unsubscribe from {Topic}", _topic);
        _logger.LogInformation("InputOrderBackgroundConsumer | Finished");
    }
}
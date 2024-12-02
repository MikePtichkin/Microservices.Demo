using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infra.Kafka.Producers.Orders;

public class OrdersInputEventPublisher : IOrdersInputEventPublisher
{
    private readonly IKafkaProducer _kafkaProduccer;
    private const string Topic = "orders_input";

    public OrdersInputEventPublisher(
        IKafkaProducer kafkaProduccer)
    {
        _kafkaProduccer = kafkaProduccer;
    }

    public Task PublishToKafka(
        OrdersInputMessage message,
        CancellationToken token)
    {
        var key = message.CustomerId;
        var body = JsonSerializer.Serialize(message);

        return _kafkaProduccer.SendMessage(
            key,
            body,
            Topic,
            token);
    }
}

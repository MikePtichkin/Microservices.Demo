using System.Threading.Tasks;
using System.Threading;

namespace Microservices.Demo.ClientOrders.Infra.Kafka.Producers;

public interface IKafkaProducer
{
    Task SendMessage(
        long key,
        string body,
        string topic,
        CancellationToken token);
}

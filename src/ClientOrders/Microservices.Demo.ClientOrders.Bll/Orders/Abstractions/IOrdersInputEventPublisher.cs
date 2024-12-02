using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;

public interface IOrdersInputEventPublisher
{
    Task PublishToKafka(
        OrdersInputMessage message,
        CancellationToken token);
}

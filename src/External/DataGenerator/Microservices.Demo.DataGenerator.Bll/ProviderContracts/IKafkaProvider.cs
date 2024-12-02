using Microservices.Demo.DataGenerator.Messages;

namespace Microservices.Demo.DataGenerator.Bll.ProviderContracts;

public interface IKafkaProvider
{
    public Task Publish(OrderInputMessage message, CancellationToken token);
}

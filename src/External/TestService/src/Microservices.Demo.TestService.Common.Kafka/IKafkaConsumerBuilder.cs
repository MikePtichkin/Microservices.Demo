using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.TestService.Common.Kafka;

public interface IKafkaConsumerBuilder<TKey, TValue>
    where TValue : class
{
    IServiceCollection Services { get; }
}

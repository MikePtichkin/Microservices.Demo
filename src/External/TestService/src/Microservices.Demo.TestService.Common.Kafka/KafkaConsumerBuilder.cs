using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.TestService.Common.Kafka;

public class KafkaConsumerBuilder<TKey, TValue> : IKafkaConsumerBuilder<TKey, TValue>
    where TKey : class
    where TValue : class
{
    public KafkaConsumerBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}

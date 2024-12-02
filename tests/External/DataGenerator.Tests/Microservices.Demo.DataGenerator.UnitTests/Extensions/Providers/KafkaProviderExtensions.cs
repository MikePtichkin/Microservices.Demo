using Moq;
using Microservices.Demo.DataGenerator.Bll.ProviderContracts;
using Microservices.Demo.DataGenerator.Messages;

namespace Microservices.Demo.DataGenerator.UnitTests.Extensions.Providers;

public static class KafkaProviderExtensions
{
    public static Mock<IKafkaProvider> VerifyPublish(
        this Mock<IKafkaProvider> mock,
        OrderInputMessage expected,
        int times = 1)
    {
        mock.Verify(
            provider => provider.Publish(
                It.Is<OrderInputMessage>(actual => actual.JsonCompare(expected)),
                It.IsAny<CancellationToken>()),
            Times.Exactly(times));

        return mock;
    }
}

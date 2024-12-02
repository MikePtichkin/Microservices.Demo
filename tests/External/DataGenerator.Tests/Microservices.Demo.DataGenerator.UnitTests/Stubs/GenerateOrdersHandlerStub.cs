using Moq;
using Microservices.Demo.DataGenerator.Bll.Mediator.Handlers;
using Microservices.Demo.DataGenerator.Bll.ProviderContracts;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.UnitTests.Stubs;

public class GenerateOrdersHandlerStub : GenerateOrdersHandler
{
    internal Mock<IKafkaProvider> KafkaProvider { get; }

    internal Mock<ICustomerProvider> CustomerProvider { get; }

    internal Mock<ICustomerService> CustomerService { get; }

    internal Mock<IOrderService> OrderService { get; }

    internal Mock<IBrokenOrderService> BrokenOrderService { get; }

    public GenerateOrdersHandlerStub(
        Mock<IKafkaProvider> kafkaProvider,
        Mock<ICustomerProvider> customerProvider,
        Mock<ICustomerService> customerService,
        Mock<IOrderService> orderService,
        Mock<IBrokenOrderService> brokenOrderService) : base(
        kafkaProvider.Object,
        customerProvider.Object,
        customerService.Object,
        orderService.Object,
        brokenOrderService.Object)
    {
        KafkaProvider = kafkaProvider;
        CustomerProvider = customerProvider;
        CustomerService = customerService;
        OrderService = orderService;
        BrokenOrderService = brokenOrderService;
    }

    public void VerifyNoOtherCalls()
    {
        KafkaProvider.VerifyNoOtherCalls();
        CustomerProvider.VerifyNoOtherCalls();
        CustomerService.VerifyNoOtherCalls();
        OrderService.VerifyNoOtherCalls();
        BrokenOrderService.VerifyNoOtherCalls();
    }
}

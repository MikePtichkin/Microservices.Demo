using Moq;
using Microservices.Demo.OrderService.Bll.Helpers.Interfaces;
using Microservices.Demo.OrderService.Bll.Services.Interfaces;
using Microservices.Demo.OrderService.Dal.Interfaces;
using Microservices.Demo.OrderService.Dal.Repositories;
using Microservices.Demo.OrderService.Kafka;

namespace Microservices.Demo.OrderService.UnitTests;

public class Mocks
{
    public readonly Mock<IOrdersRepository> OrdersRepositoryMock = new();
    public readonly Mock<IItemsRepository> ItemsRepositoryMock = new();
    public readonly Mock<IInputValidationHelper> InputValidatorHelperMock = new();
    public readonly Mock<ILogsService> LogsServiceMock = new();
    public readonly Mock<IOrderInputErrorsPublisher> OrderInputErrorsPublisherMock = new();
    public readonly Mock<IOrderOutputEventPublisher> OrderOutputEventPublisherMock = new();
}
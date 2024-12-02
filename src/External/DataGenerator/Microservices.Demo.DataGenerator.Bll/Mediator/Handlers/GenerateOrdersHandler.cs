using MediatR;
using Microservices.Demo.DataGenerator.Bll.Mapping;
using Microservices.Demo.DataGenerator.Bll.Mediator.Commands;
using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Bll.ProviderContracts;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.Bll.Mediator.Handlers;

public class GenerateOrdersHandler : IRequestHandler<GenerateOrdersCommand, int>
{
    private readonly IKafkaProvider _kafkaProvider;
    private readonly ICustomerProvider _customerProvider;
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    private readonly IBrokenOrderService _brokenOrderService;

    public GenerateOrdersHandler(
        IKafkaProvider kafkaProvider,
        ICustomerProvider customerProvider,
        ICustomerService customerService,
        IOrderService orderService,
        IBrokenOrderService brokenOrderService)
    {
        _kafkaProvider = kafkaProvider;
        _customerProvider = customerProvider;
        _customerService = customerService;
        _orderService = orderService;
        _brokenOrderService = brokenOrderService;
    }

    public async Task<int> Handle(GenerateOrdersCommand request, CancellationToken token)
    {
        if (request.CustomersCount == 0)
        {
            return default;
        }

        var customers = await CreateCustomers(request.CustomersCount, token);

        if (request.OrdersCount == 0 || customers.Count == 0)
        {
            return default;
        }

        var orders = CreateOrders(
            request.OrdersCount,
            customers,
            request.BrokenOrdersIndexes);

        await SendMessages(orders, token);

        return orders.Count;
    }

    private async Task<IReadOnlyList<Customer>> CreateCustomers(
        int customersCount,
        CancellationToken token)
    {
        var customerModels = _customerService.Create(customersCount);
        var customers = new List<Customer>(customersCount);

        foreach (var customer in customerModels)
        {
            var customerId = await _customerProvider.CreateCustomer(customer, token);

            if (customerId is null)
            {
                continue;
            }

            customers.Add(customer with { Id = customerId });
        }

        return customers;
    }

    private IReadOnlyList<Order> CreateOrders(
        int ordersCount,
        IReadOnlyList<Customer> customers,
        IReadOnlyList<int> brokenOrdersIndexes)
    {
        var orders = Enumerable.Range(0, ordersCount)
            .Select(_ => _orderService.Create(customers))
            .ToArray();

        if (!brokenOrdersIndexes.Any())
        {
            return orders;
        }

        foreach (var index in brokenOrdersIndexes)
        {
            orders[index] = _brokenOrderService.BreakOrder(orders[index]);
        }

        return orders;
    }

    private async Task SendMessages(IReadOnlyList<Order> orders, CancellationToken token)
    {
        foreach (var order in orders)
        {
            await _kafkaProvider.Publish(order.Map(), token);
        }
    }
}

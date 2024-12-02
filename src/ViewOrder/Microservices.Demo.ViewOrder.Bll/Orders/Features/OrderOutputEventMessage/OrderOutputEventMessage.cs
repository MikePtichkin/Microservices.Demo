using MediatR;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Features.OrderOutputEventMessage;

public sealed record OrderOutputEventMessage(long OrderId) : IRequest;
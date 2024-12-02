using Microservices.Demo.OrderService.Kafka.Messages;

namespace Microservices.Demo.OrderService.Bll.Models;

public record ValidationResult(
    bool Success,
    OrderInputErrorsMessage Message);
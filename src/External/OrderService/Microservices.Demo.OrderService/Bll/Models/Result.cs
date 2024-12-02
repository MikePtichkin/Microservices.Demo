namespace Microservices.Demo.OrderService.Bll.Models;

public record Result(
    bool Success,
    ActionError? Error);
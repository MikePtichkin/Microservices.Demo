namespace Microservices.Demo.OrderService.Bll.Models;

public record ActionError(
    string ErrorCode,
    string ErrorMessage);
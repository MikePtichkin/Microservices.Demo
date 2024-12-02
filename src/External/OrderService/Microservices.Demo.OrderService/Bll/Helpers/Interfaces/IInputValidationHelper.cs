using Microservices.Demo.OrderService.Bll.Models;

namespace Microservices.Demo.OrderService.Bll.Helpers.Interfaces;

public interface IInputValidationHelper
{
    Task<ValidationResult> ValidateInputOrder(
        InputOrder inputOrder,
        CancellationToken token);
}
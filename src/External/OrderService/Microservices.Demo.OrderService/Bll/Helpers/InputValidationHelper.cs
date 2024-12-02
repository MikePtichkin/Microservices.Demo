using Microservices.Demo.OrderService.Bll.Exceptions;
using Microservices.Demo.OrderService.Bll.Helpers.Interfaces;
using Microservices.Demo.OrderService.Bll.Models;
using Microservices.Demo.OrderService.Dal.Interfaces;
using Microservices.Demo.OrderService.Kafka.Extensions;
using ValidationResult = Microservices.Demo.OrderService.Bll.Models.ValidationResult;

namespace Microservices.Demo.OrderService.Bll.Helpers;

public class InputValidationHelper : IInputValidationHelper
{
    private readonly IRegionRepository _regionRepository;

    public InputValidationHelper(
        IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public async Task<ValidationResult> ValidateInputOrder(
        InputOrder inputOrder,
        CancellationToken token)
    {
        if (inputOrder.Items is not { Length: > 0 })
        {
            return ReturnFailed(
                inputOrder,
                error: nameof(EmptyOrderException),
                errorMessage: "Order without items");
        }

        var emptyItems = inputOrder.Items.Where(item => item.Quantity <= 0).ToArray();
        if (emptyItems is { Length: > 0 })
        {
            return ReturnFailed(
                inputOrder,
                error: nameof(InvalidItemsCountException),
                errorMessage: "Quantity of the items must be greater than 0");
        }
        
        var region = await _regionRepository.Get(inputOrder.RegionId, token);

        if (region is null)
        {
            return ReturnFailed(
                inputOrder,
                error: nameof(UnsupportedRegionException),
                errorMessage: $"Unsupported region. RegionId: {inputOrder.RegionId}");
        }

        return new ValidationResult(Success: true, Message: null);
    }

    private ValidationResult ReturnFailed(
        InputOrder inputOrder,
        string error, 
        string errorMessage)
    {
        return new ValidationResult(
            Success: false,
            Message: inputOrder.ToInputErrorMessage(
                reason: error,
                description: errorMessage));
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Dtos;

public record OrderWithCustomerInfoDto(
    long Id,
    long RegionId,
    string Status,
    long CustomerId,
    [Required] string CustomerName,
    string? Comment,
    DateTime CreatedAt);

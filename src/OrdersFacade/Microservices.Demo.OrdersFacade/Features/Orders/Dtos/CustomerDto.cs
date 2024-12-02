using System;
using System.ComponentModel.DataAnnotations;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Dtos;

public record CustomerDto(
    long Id,
    long RegionId,
    [Required] string Name,
    DateTime CreatedAt);
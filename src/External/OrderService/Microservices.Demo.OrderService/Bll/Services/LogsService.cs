using Microservices.Demo.OrderService.Bll.Models;
using Microservices.Demo.OrderService.Bll.Services.Interfaces;
using Microservices.Demo.OrderService.Dal.Entities;
using Microservices.Demo.OrderService.Dal.Interfaces;

namespace Microservices.Demo.OrderService.Bll.Services;

public class LogsService : ILogsService
{
    private readonly ILogsRepository _logsRepository;

    public LogsService(
        ILogsRepository logsRepository)
    {
        _logsRepository = logsRepository;
    }

    public async Task AddLog(
        Order order, 
        CancellationToken token)
    {
        await _logsRepository.Insert(
            new OrderLogEntity
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                RegionId = order.Region.Id,
                Status = (int)order.Status,
                Comment = order.Comment,
                CreatedAt = order.CreatedAt,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            token);
    }
}
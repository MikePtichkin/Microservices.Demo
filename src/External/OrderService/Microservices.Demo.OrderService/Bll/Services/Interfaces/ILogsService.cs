using Microservices.Demo.OrderService.Bll.Models;

namespace Microservices.Demo.OrderService.Bll.Services.Interfaces;

public interface ILogsService
{
    Task AddLog(
        Order order,
        CancellationToken token);
}
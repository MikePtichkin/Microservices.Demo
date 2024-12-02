using Microservices.Demo.OrderService.Dal.Entities;

namespace Microservices.Demo.OrderService.Dal.Interfaces;

public interface ILogsRepository
{
    Task Insert(
        OrderLogEntity logEntity,
        CancellationToken token);
}
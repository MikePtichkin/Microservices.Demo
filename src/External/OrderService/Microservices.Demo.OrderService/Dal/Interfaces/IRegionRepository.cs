using Microservices.Demo.OrderService.Dal.Entities;

namespace Microservices.Demo.OrderService.Dal.Interfaces;

public interface IRegionRepository
{
    Task<RegionEntity?> Get(
        long regionId,
        CancellationToken token);
}
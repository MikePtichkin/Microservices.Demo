namespace Microservices.Demo.CustomerService.Repositories;

public interface IRegionRepository
{
    Task<Domain.Region> Get(long regionId, CancellationToken token);
}
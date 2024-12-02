using Dapper;

using Microservices.Demo.CustomerService.Domain;
using Microservices.Demo.CustomerService.Infrastructure.PostgresRepositories.Contracts;
using Microservices.Demo.CustomerService.Repositories;
using Microservices.Demo.CustomerService.Repositories.Exceptions;

namespace Microservices.Demo.CustomerService.Infrastructure.PostgresRepositories;

public class RegionRepository : BaseRepository, IRegionRepository
{
    public async Task<Region> Get(long regionId, CancellationToken token)
    {
        const string sqlQuery = @"
                SELECT id, name
                FROM regions 
                WHERE id = @RegionId;";

        var param = new DynamicParameters();
        param.Add("RegionId", regionId);

        var record = (await ExecuteQueryAsync<RegionRecord>(sqlQuery, param, token)).FirstOrDefault();

        if (record is null)
        {
            throw new RegionNotFoundException($"Invalid region id. The region with the '{regionId}' id is not found.");
        }

        return new Region(record.Id, record.Name);
    }
}
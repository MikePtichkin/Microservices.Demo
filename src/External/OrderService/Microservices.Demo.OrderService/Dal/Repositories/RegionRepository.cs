using Dapper;
using Microservices.Demo.OrderService.Dal.Entities;
using Microservices.Demo.OrderService.Dal.Interfaces;

namespace Microservices.Demo.OrderService.Dal.Repositories;

public class RegionRepository : BaseRepository, IRegionRepository
{
    public RegionRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<RegionEntity?> Get(
        long regionId,
        CancellationToken token)
    {
        const string sqlQuery = @"
                select 
                    id,
                    name
                from 
                    regions 
                where 
                    id = @RegionId;";

        var param = new DynamicParameters();
        param.Add("RegionId", regionId);
        
        var result = await ExecuteQueryAsync<RegionEntity>(sqlQuery, param, token);
        return result.FirstOrDefault();
    }
}
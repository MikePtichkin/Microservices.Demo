using Microsoft.EntityFrameworkCore;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Common;

internal sealed class ClientOrdersDbContext : DbContext
{
    public ClientOrdersDbContext(DbContextOptions<ClientOrdersDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientOrdersDbContext).Assembly);
    }
}

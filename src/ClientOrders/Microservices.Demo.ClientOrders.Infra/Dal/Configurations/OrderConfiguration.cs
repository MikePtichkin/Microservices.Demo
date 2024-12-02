using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservices.Demo.ClientOrders.Domain.Orders;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id");

        builder.Property(o => o.OrderId)
            .HasColumnName("order_id")
            .IsRequired(required: false);

        builder.Property(o => o.RegionId)
            .HasColumnName("region_id")
            .IsRequired();

        builder.Property(o => o.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new Domain.Common.TimeStamp { Value = v.ToLocalTime() });

        builder.Property(o => o.Comment)
            .HasColumnName("comment")
            .HasConversion(
                v => v.Value,
                v => new OrderComment { Value = v });

        builder.Property(o => o.Error)
            .HasColumnName("error");
    }
}

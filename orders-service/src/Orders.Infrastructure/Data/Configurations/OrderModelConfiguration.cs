using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Infrastructure.Data.Entities;

namespace Orders.Infrastructure.Data.Configurations;

internal static class OrderModelConfiguration
{
    public static void ConfigureModel(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasOne(e => e.ProductSnapshot)
            .WithOne(e => e.Order)
            .HasForeignKey<OrderEntity>(e => e.ProductSnapshotId);
    }
}

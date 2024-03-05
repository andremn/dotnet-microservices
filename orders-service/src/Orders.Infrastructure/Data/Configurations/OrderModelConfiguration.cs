using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Data.Configurations;

internal static class OrderModelConfiguration
{
    public static void ConfigureModel(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(e => e.ProductSnapshot)
            .WithOne(e => e.Order)
            .HasForeignKey<Order>(e => e.ProductSnapshotId);
    }
}

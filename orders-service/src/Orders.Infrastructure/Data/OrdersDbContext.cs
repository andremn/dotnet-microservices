using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Data.Configurations;
using Orders.Infrastructure.Data.Entities;

namespace Orders.Infrastructure.Data;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<ProductSnapshotEntity> ProductSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        OrderModelConfiguration.ConfigureModel(modelBuilder.Entity<OrderEntity>());
    }
}

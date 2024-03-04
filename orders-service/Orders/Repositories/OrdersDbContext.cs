using Microsoft.EntityFrameworkCore;
using Orders.Repositories.Entities;

namespace Orders.Repositories;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<ProductSnapshotEntity> ProductSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        OrderEntity.ConfigureModel(modelBuilder);
    }
}

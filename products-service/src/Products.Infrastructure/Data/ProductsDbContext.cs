using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Data.Entities;

namespace Products.Infrastructure.Data;

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; set; }
}

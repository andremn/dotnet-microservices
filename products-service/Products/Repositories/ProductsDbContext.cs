using Microsoft.EntityFrameworkCore;
using Products.Repositories.Entities;

namespace Products.Repositories;

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; set; }
}

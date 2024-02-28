using Microsoft.EntityFrameworkCore;

namespace Products.Repositories;

public class ProductsDbContext(DbContextOptions options) : DbContext(options)
{
}

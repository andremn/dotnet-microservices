using Microsoft.EntityFrameworkCore;
using Products.Extensions;
using Products.Model;

namespace Products.Repositories;

public class ProductRepository(ProductsDbContext dbContext) : IProductRepository
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public async Task<IList<Product>> GetAllAsync() =>
        await _dbContext.Products
        .Select(x => x.ToModel())
        .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
    {
        var entity = await _dbContext.Products
            .SingleOrDefaultAsync(x => x.Id == id);

        return entity?.ToModel();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var entity = product.ToEntity();

        _dbContext.Products.Add(entity);

        await _dbContext.SaveChangesAsync();

        return product with { Id = entity.Id };
    }
}

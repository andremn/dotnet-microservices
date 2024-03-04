using Microsoft.EntityFrameworkCore;
using Products.Extensions;
using Products.Model;
using System.Data;

namespace Products.Repositories;

public class ProductRepository(ProductsDbContext dbContext) : IProductRepository
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public async Task<IList<Product>> GetAllAsync() =>
        await _dbContext.Products
        .AsNoTracking()
        .Select(x => x.ToModel())
        .ToListAsync();

    public async Task<IList<Product>> GetAllByIdsAsync(IEnumerable<int> ids) =>
        await _dbContext.Products
        .AsNoTracking()
        .Where(x => ids.Any(y => y == x.Id))
        .Select(x => x.ToModel())
        .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
    {
        var entity = await _dbContext.Products
            .AsNoTracking()
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

    public async Task UpdateAsync(Product product)
    {
        var entity = product.ToEntity();

        _dbContext.Products.Update(entity);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateQuantityAsync(int id, int quantity)
    {
        var updatedProducts = await _dbContext.Products
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, v => v.Quantity + quantity));

        return updatedProducts > 0;
    }

    public async Task DeleteAsync(Product product)
    {
        var entity = product.ToEntity();

        _dbContext.Products.Remove(entity);

        await _dbContext.SaveChangesAsync();
    }
}

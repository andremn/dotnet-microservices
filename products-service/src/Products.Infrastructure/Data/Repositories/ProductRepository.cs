using Microsoft.EntityFrameworkCore;
using System.Data;
using Products.Domain.Repository;
using Products.Domain.Models;
using Products.Infrastructure.Mapping;

namespace Products.Infrastructure.Data.Repositories;

public class ProductRepository(ProductsDbContext dbContext) : IProductRepository
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public async Task<IList<Product>> GetAllAsync() =>
        await _dbContext.Products
        .AsNoTracking()
        .Select(x => x.ToModel())
        .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
    {
        var entity = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity?.ToModel();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var entity = product.ToEntity();

        _dbContext.Products.Add(entity);

        await _dbContext.SaveChangesAsync();

        return product with { Id = entity.Id };
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var entity = await GetByIdAsync(product.Id);

        if (entity is null)
        {
            return null;
        }

        var entityToUpdate = product.ToEntity();

        _dbContext.Products.Update(entityToUpdate);

        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task IncrementQuantityAsync(int id, int quantity)
    {
        await _dbContext.Products
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, v => v.Quantity + quantity));
    }

    public async Task DeleteAsync(Product product)
    {
        var entity = product.ToEntity();

        _dbContext.Products.Remove(entity);

        await _dbContext.SaveChangesAsync();
    }
}

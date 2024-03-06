using Products.Domain.Models;

namespace Products.Domain.Repository;
public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);

    Task<IList<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<Product?> UpdateAsync(Product product);

    Task IncrementQuantityAsync(int id, int quantity);

    Task DeleteAsync(Product product);
}
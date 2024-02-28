using Products.Model;

namespace Products.Repositories;
public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);

    Task<IList<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);
}
using Products.Model;

namespace Products.Repositories;
public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);

    Task<IList<Product>> GetAllAsync();

    Task<IList<Product>> GetAllByIdsAsync(IEnumerable<int> ids);

    Task<Product?> GetByIdAsync(int id);

    Task UpdateAsync(Product product);

    Task<bool> UpdateQuantityAsync(int id, int quantity);

    Task DeleteAsync(Product product);
}
using Products.Model;
using Products.Services.Results;

namespace Products.Services;

public interface IProductService
{
    Task<IList<Product>> GetAllAsync();

    Task<Product?> FindByIdAsync(int id);

    Task<CreateProductResult> CreateAsync(Product product);

    Task<UpdateProductResult> UpdateAsync(Product product);

    Task<UpdateProductResult> IncrementQuantityAsync(int id, int quantity);

    Task<DeleteProductResult> DeleteByIdAsync(int id);
}

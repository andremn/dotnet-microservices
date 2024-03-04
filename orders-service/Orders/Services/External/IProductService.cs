using Orders.Model;
using Refit;

namespace Orders.Services.External;

public interface IProductService
{
    [Get("/api/products")]
    Task<IList<Product>> GetAllByIdsAsync([Query(CollectionFormat.Multi)] IEnumerable<int> ids, [Header("Authorization")] string authorization);

    [Get("/api/products/{id}")]
    Task<ApiResponse<Product>> GetByIdAsync(int id, [Header("Authorization")] string authorization);

    [Patch("/api/products/{id}/quantity")]
    Task<ApiResponse<Product>> UpdateQuantityAsync(int id, [Body] UpdateProductQuantityRequest quantity, [Header("Authorization")] string authorization);
}

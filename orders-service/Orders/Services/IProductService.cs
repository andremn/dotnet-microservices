using Orders.Model;
using Refit;

namespace Orders.Services;

public interface IProductService
{
    [Get("/api/products")]
    Task<IList<Product>> GetAllByIdsAsync([Query(CollectionFormat.Multi)] IEnumerable<int> ids, [Header("Authorization")] string authorization);

    [Get("/api/products/{id}")]
    Task<ApiResponse<Product>> GetByIdAsync(int id, [Header("Authorization")] string authorization);
}

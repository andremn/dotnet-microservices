using Orders.Application.Dtos;
using Refit;

namespace Orders.Application.Services.Interfaces;

public interface IProductService
{
    [Get("/api/products")]
    Task<IList<ProductDto>> GetAllByIdsAsync([Query(CollectionFormat.Multi)] IEnumerable<int> ids, [Header("Authorization")] string authorization);

    [Get("/api/products/{id}")]
    Task<ApiResponse<ProductDto>> GetByIdAsync(int id, [Header("Authorization")] string authorization);

    [Patch("/api/products/{id}/quantity")]
    Task<ApiResponse<ProductDto>> UpdateQuantityAsync(int id, [Body] UpdateProductQuantityRequest quantity, [Header("Authorization")] string authorization);
}

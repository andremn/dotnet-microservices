using Orders.Application.Dtos;
using Refit;

namespace Orders.Application.Services.Interfaces;

public interface IProductService
{
    [Get("/api/products")]
    Task<IList<ProductDto>> GetAllByIdsAsync([Query(CollectionFormat.Multi)] IEnumerable<int> ids);

    [Get("/api/products/{id}")]
    Task<ApiResponse<ProductDto>> GetByIdAsync(int id);

    [Patch("/api/products/{id}/quantity")]
    Task<ApiResponse<ProductDto>> UpdateQuantityAsync(int id, [Body] UpdateProductQuantityRequest quantity);
}

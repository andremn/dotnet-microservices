using Orders.Model;
using Refit;

namespace Orders.Services;

public interface IProductService
{
    [Get("/api/products")]
    Task<IList<Product>> GetAllByIds([Query(CollectionFormat.Multi)] IEnumerable<int> ids, [Header("Authorization")] string authorization);
}

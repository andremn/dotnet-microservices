using Orders.Model;
using Orders.Services.Results;

namespace Orders.Services;
public interface IOrderService
{
    Task<IList<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(int id);

    Task<CreateOrderResult> CreateAsync(int productId, int quantity);

    Task<UpdateOrderResult> UpdateAsync(Order order);
}
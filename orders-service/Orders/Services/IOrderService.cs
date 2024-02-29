using Orders.Model;
using Orders.Services.Results;

namespace Orders.Services;
public interface IOrderService
{
    Task<IList<DetailedOrder>> GetAllAsync();

    Task<GetDetailedOrderResult> GetByIdAsync(int id);

    Task<CreateOrderResult> CreateAsync(int productId);

    Task<UpdateOrderResult> UpdateAsync(Order order);
}
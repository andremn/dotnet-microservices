using Orders.Application.Services.Results;
using Orders.Domain.Models;

namespace Orders.Application.Services.Interfaces;

public interface IOrderService
{
    Task<IList<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(int id);

    Task<CreateOrderResult> CreateAsync(int productId, int quantity);
}
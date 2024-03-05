using Orders.Application.Services.Results;
using Orders.Domain.Dtos;

namespace Orders.Application.Services.Interfaces;

public interface IOrderService
{
    Task<IList<OrderDto>> GetAllAsync();

    Task<OrderDto?> GetByIdAsync(int id);

    Task<CreateOrderResult> CreateAsync(int productId, int quantity);
}
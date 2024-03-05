using Orders.Domain.Dtos;

namespace Orders.Domain.Repositories;

public interface IOrderRepository
{
    Task<IList<OrderDto>> GetAllByUserAsync(string userId);

    Task<OrderDto?> GetByIdAsync(int id);

    Task<OrderDto> CreateAsync(OrderDto order);

    Task<OrderDto> UpdateAsync(OrderDto order);
}
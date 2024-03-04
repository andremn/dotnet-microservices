using Orders.Model;

namespace Orders.Repositories;
public interface IOrderRepository
{
    Task<IList<Order>> GetAllByUserAsync(string userId);

    Task<Order?> GetByIdAsync(int id);

    Task<Order> CreateAsync(Order order);

    Task<Order> UpdateAsync(Order order);
}
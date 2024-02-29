using Orders.Model;

namespace Orders.Repositories;
public interface IOrderRepository
{
    Task<IList<Order>> GetAllByUserAsync(string userEmail);

    Task<Order?> GetByIdAsync(int id);

    Task<Order> CreateAsync(Order order);
}
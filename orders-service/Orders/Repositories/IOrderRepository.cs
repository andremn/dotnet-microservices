using Orders.Model;

namespace Orders.Repositories;
public interface IOrderRepository
{
    Task<IList<Order>> GetAllByUserAsync(string userEmail);
}
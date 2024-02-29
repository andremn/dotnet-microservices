using Orders.Model;

namespace Orders.Services;
public interface IOrderService
{
    Task<IList<DetailedOrder>> GetAllAsync();
}
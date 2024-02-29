using Orders.Model;

namespace Orders.Services;

public interface IShippingService
{
    Task RequestOrderShippingAsync(Order order);
}

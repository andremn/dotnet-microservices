using Orders.Model;

namespace Orders.Services.External;

public interface IShippingService
{
    Task RequestOrderShippingAsync(Order order);

    Task HandleShippingRequestAsync(int orderId);
}

using Orders.Model;

namespace Orders.Services;

public class ShippingService : IShippingService
{

    public Task RequestOrderShippingAsync(Order order)
    {
        // Fake shipping service implementation simulating the shipping request was sent
        return Task.CompletedTask;
    }
}

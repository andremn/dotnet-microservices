using Orders.Domain.Models;
namespace Orders.Application.Services.Interfaces;

public interface IShippingService
{
    Task RequestOrderShippingAsync(Order order);

    Task HandleShippingRequestAsync(int orderId);
}

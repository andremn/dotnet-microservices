using Orders.Domain.Dtos;

namespace Orders.Application.Services.Interfaces;

public interface IShippingService
{
    Task RequestOrderShippingAsync(OrderDto order);

    Task HandleShippingRequestAsync(int orderId);
}

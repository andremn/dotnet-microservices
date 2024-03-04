using Orders.Model;

namespace Orders.Services.Orders;
public interface IOrderProcessingService
{
    Task HandleOrderCreatedAsync(Order order);

    Task HandlePaymentStatusChangedAsync(int orderId, OrderPaymentStatus status);

    Task HandleShippingStatusChangedAsync(int orderId, OrderShippingStatus status);
}
using Orders.Model;

namespace Orders.Services;
public interface IOrderProcessingService
{
    Task HandleDeliverStatusChangedAsync(int orderId, bool delivered);

    Task HandleOrderChangedAsync(Order order);

    Task HandlePaymentChangedAsync(int orderId, bool approved);

    Task HandleShippingStartedAsync(int orderId);
}
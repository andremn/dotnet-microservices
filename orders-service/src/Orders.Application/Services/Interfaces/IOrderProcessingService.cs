using Orders.Application.Enums;
using Orders.Domain.Models;

namespace Orders.Application.Services.Interfaces;

public interface IOrderProcessingService
{
    Task HandleOrderCreatedAsync(Order order);

    Task HandlePaymentStatusChangedAsync(int orderId, OrderPaymentStatus status);

    Task HandleShippingStatusChangedAsync(int orderId, OrderShippingStatus status);
}
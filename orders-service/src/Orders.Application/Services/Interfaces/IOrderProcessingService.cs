using Orders.Application.Enums;
using Orders.Domain.Dtos;

namespace Orders.Application.Services.Interfaces;

public interface IOrderProcessingService
{
    Task HandleOrderCreatedAsync(OrderDto order);

    Task HandlePaymentStatusChangedAsync(int orderId, OrderPaymentStatus status);

    Task HandleShippingStatusChangedAsync(int orderId, OrderShippingStatus status);
}
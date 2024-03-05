using Microsoft.Extensions.Logging;
using Orders.Application.Enums;
using Orders.Application.Services.Interfaces;
using Orders.Domain.Enums;
using Orders.Domain.Models;
using Orders.Domain.Repositories;

namespace Orders.Application.Services;

public class OrderProcessingService(
    IPaymentService paymentService,
    IShippingService shippingService,
    IOrderRepository orderRepository,
    ILogger<OrderProcessingService> logger
) : IOrderProcessingService
{
    public async Task HandleOrderCreatedAsync(Order order)
    {
        await paymentService.SendApprovalRequestAsync(order);

        var updatedOrder = order with { Status = OrderStatus.AwaitingPayment };

        await orderRepository.UpdateAsync(updatedOrder);
    }

    public async Task HandlePaymentStatusChangedAsync(int orderId, OrderPaymentStatus status)
    {
        var existingOrder = await orderRepository.GetByIdAsync(orderId);

        if (existingOrder is null)
        {
            logger.LogWarning("Payment status changed, but order with id '{orderId}' was not found", orderId);
            return;
        }

        var newStatus = status switch
        {
            OrderPaymentStatus.AwaitingApproval => OrderStatus.AwaitingPayment,
            OrderPaymentStatus.Approved => OrderStatus.PaymentConfirmed,
            OrderPaymentStatus.Denied => OrderStatus.PaymentDenied,
            _ => existingOrder.Status
        };

        var updatedOrder = await UpdateOrderStatusAsync(existingOrder, newStatus);

        await shippingService.RequestOrderShippingAsync(updatedOrder);
    }

    public async Task HandleShippingStatusChangedAsync(int orderId, OrderShippingStatus status)
    {
        var existingOrder = await orderRepository.GetByIdAsync(orderId);

        if (existingOrder is null)
        {
            logger.LogWarning("Shipping was started, but order with id '{orderId}' was not found", orderId);
            return;
        }

        var orderStatus = status switch
        {
            OrderShippingStatus.AwaitingCollect => OrderStatus.AwaitingShipping,
            OrderShippingStatus.Collected => OrderStatus.Shipped,
            OrderShippingStatus.EnRoute => OrderStatus.Shipped,
            OrderShippingStatus.Delivered => OrderStatus.Finished,
            OrderShippingStatus.NotDelivered => OrderStatus.DeliveryFailed,
            _ => existingOrder.Status
        };

        await UpdateOrderStatusAsync(existingOrder, orderStatus);
    }

    private async Task<Order> UpdateOrderStatusAsync(Order order, OrderStatus newStatus)
    {
        var updatedOrder = order with { Status = newStatus };

        await orderRepository.UpdateAsync(updatedOrder);

        return updatedOrder;
    }
}

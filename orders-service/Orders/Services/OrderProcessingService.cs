using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;
using Orders.Repositories;

namespace Orders.Services;

public class OrderProcessingService(
    IPaymentService paymentService,
    IShippingService shippingService,
    IPublisher<OrderChangeMessage> orderChangePublisher,
    IOrderRepository orderRepository,
    ILogger<OrderProcessingService> logger
) : IOrderProcessingService
{
    public async Task HandlePaymentChangedAsync(int orderId, bool approved)
    {
        var existingOrder = await orderRepository.GetByIdAsync(orderId);

        if (existingOrder is null)
        {
            logger.LogWarning("Payment status changed, but order with id '{orderId}' was not found", orderId);
            return;
        }

        var newStatus = approved ? OrderStatus.PaymentConfirmed : OrderStatus.PaymentDenied;
        var updatedOrder = await UpdateOrderStatusAsync(existingOrder, newStatus);

        PublishOrderChangeMessage(updatedOrder);
    }

    public async Task HandleShippingStartedAsync(int orderId)
    {
        var existingOrder = await orderRepository.GetByIdAsync(orderId);

        if (existingOrder is null)
        {
            logger.LogWarning("Shipping was started, but order with id '{orderId}' was not found", orderId);
            return;
        }

        var updatedOrder = await UpdateOrderStatusAsync(existingOrder, OrderStatus.Shipped);

        PublishOrderChangeMessage(updatedOrder);
    }

    public async Task HandleDeliverStatusChangedAsync(int orderId, bool delivered)
    {
        var existingOrder = await orderRepository.GetByIdAsync(orderId);

        if (existingOrder is null)
        {
            logger.LogWarning("Deliver status changed, but order with id '{orderId}' was not found", orderId);
            return;
        }

        var newStatus = delivered ? OrderStatus.Finished : OrderStatus.DeliveryFailed;
        var updatedOrder = await UpdateOrderStatusAsync(existingOrder, newStatus);

        PublishOrderChangeMessage(updatedOrder);
    }

    public async Task HandleOrderChangedAsync(Order order)
    {
        logger.LogDebug("Order '{orderId}' changed: {order}", order.Id, order);

        var newStatus = order.Status;

        if (order.Status is OrderStatus.Created)
        {
            await paymentService.SendApprovalRequestAsync(order);

            newStatus = OrderStatus.AwaitingPayment;
        }
        else if (order.Status is OrderStatus.PaymentConfirmed)
        {
            await shippingService.RequestOrderShippingAsync(order);

            newStatus = OrderStatus.AwaitingShipping;
        }

        if (order.Status != newStatus)
        {
            var updatedOrder = order with { Status = newStatus };

            await orderRepository.UpdateAsync(updatedOrder);
        }
    }

    private async Task<Order> UpdateOrderStatusAsync(Order order, OrderStatus newStatus)
    {
        var updatedOrder = order with { Status = newStatus };

        await orderRepository.UpdateAsync(updatedOrder);

        return updatedOrder;
    }

    private void PublishOrderChangeMessage(Order order) =>
        orderChangePublisher.Publish(order.ToChangeMessage());
}

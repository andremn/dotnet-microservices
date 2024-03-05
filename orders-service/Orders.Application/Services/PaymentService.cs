using Microsoft.Extensions.Logging;
using Orders.Application.Enums;
using Orders.Application.Mapping;
using Orders.Application.Messaging.Messages;
using Orders.Application.Messaging.Publishers;
using Orders.Application.Services.Interfaces;
using Orders.Domain.Dtos;

namespace Orders.Application.Services;

public class PaymentService(
    IPublisher<OrderPaymentRequestMessage> publisher,
    IPublisher<OrderPaymentStatusChangedMessage> paymentStatusPublisher,
    ILogger<PaymentService> logger
) : IPaymentService
{
    public Task SendApprovalRequestAsync(OrderDto order)
    {
        publisher.Publish(order.ToPaymentRequestMessage());

        return Task.CompletedTask;
    }

    public Task HandlePaymentRequest(int orderId, decimal price)
    {
        logger.LogDebug("Payment request received for order '{orderId}' with price {price}", orderId, price.ToString("C"));

        paymentStatusPublisher.Publish(new OrderPaymentStatusChangedMessage(orderId, OrderPaymentStatus.Approved));

        return Task.CompletedTask;
    }
}

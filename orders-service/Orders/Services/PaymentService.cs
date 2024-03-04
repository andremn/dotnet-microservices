﻿using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;

namespace Orders.Services;

public class PaymentService(
    IPublisher<OrderPaymentRequestMessage> publisher,
    IPublisher<OrderPaymentStatusChangedMessage> paymentStatusPublisher,
    ILogger<PaymentService> logger
) : IPaymentService
{
    public Task SendApprovalRequestAsync(Order order)
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

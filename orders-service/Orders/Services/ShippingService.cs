﻿using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;

namespace Orders.Services;

public class ShippingService(
    IPublisher<OrderShippingRequestMessage> shippingRequestPublisher,
    IPublisher<OrderShippingStatusChangedMessage> shippingStatusChangedPublisher,
    ILogger<ShippingService> logger
) : IShippingService
{
    public Task RequestOrderShippingAsync(Order order)
    {
        shippingRequestPublisher.Publish(order.ToOrderShippingRequestMessage());

        return Task.CompletedTask;
    }

    public Task HandleShippingRequestAsync(int orderId)
    {
        logger.LogDebug("Shipping request received for order '{orderId}'", orderId);

        shippingStatusChangedPublisher.Publish(new OrderShippingStatusChangedMessage(orderId, OrderShippingStatus.AwaitingCollect));

        return Task.CompletedTask;
    }
}

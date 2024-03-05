using Microsoft.Extensions.Logging;
using Orders.Application.Enums;
using Orders.Application.Mapping;
using Orders.Application.Messaging.Messages;
using Orders.Application.Messaging.Publishers;
using Orders.Application.Services.Interfaces;
using Orders.Domain.Models;

namespace Orders.Application.Services;

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

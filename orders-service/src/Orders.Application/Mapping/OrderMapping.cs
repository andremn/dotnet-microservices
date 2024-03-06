using Orders.Application.Messaging.Messages;
using Orders.Domain.Models;
using Orders.Domain.Enums;

namespace Orders.Application.Mapping;

public static class OrderMapping
{
    public static OrderCreatedMessage ToOrderCreatedMessage(this Order order) =>
        new(order.Id, order.ProductId, order.UserId, order.ProductSnapshot, order.Quantity, order.CreatedAt);

    public static OrderPaymentRequestMessage ToPaymentRequestMessage(this Order order) =>
        new(order.Id, order.ProductSnapshot.Price);

    public static OrderShippingRequestMessage ToOrderShippingRequestMessage(this Order order) =>
        new(order.Id);

    public static Order ToDto(this OrderCreatedMessage message) =>
        new(
            message.Id,
            message.ProductId,
            message.UserId,
            message.ProductSnapshot,
            message.Quantity,
            OrderStatus.Created,
            message.CreatedAt);
}

using Orders.Messaging.Messages;
using Orders.Model;
using Orders.Repositories.Entities;

namespace Orders.Extensions;

public static class OrderExtensions
{
    public static Order ToModel(this OrderEntity entity) =>
        new(entity.Id, entity.ProductId, entity.UserId, entity.ProductSnapshot.ToModel(), entity.Quantity, entity.Status, entity.CreatedAt);

    public static OrderEntity ToEntity(this Order order) =>
        new()
        {
            Id = order.Id,
            ProductId = order.ProductId,
            ProductSnapshot = order.ProductSnapshot.ToEntity(),
            UserId = order.UserId,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };

    public static OrderCreatedMessage ToOrderCreatedMessage(this Order order) =>
        new(order.Id, order.ProductId, order.UserId, order.ProductSnapshot, order.Quantity, order.CreatedAt);

    public static OrderPaymentRequestMessage ToPaymentRequestMessage(this Order order) =>
        new(order.Id, order.ProductSnapshot.Price);

    public static OrderShippingRequestMessage ToOrderShippingRequestMessage(this Order order) =>
        new(order.Id);

    public static Order ToModel(this OrderCreatedMessage message) =>
        new(
            message.Id,
            message.ProductId,
            message.UserId,
            message.ProductSnapshot,
            message.Quantity,
            OrderStatus.Created,
            message.CreatedAt);
}

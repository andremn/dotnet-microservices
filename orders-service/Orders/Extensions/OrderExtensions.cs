using Orders.Messaging.Messages;
using Orders.Model;
using Orders.Repositories.Entities;

namespace Orders.Extensions;

public static class OrderExtensions
{
    public static Order ToModel(this OrderEntity entity) =>
        new(entity.Id, entity.ProductId, entity.UserId, entity.Price, entity.Status, entity.CreatedAt);

    public static OrderEntity ToEntity(this Order order) =>
        new()
        {
            Id = order.Id,
            ProductId = order.ProductId,
            UserId = order.UserId,
            Price = order.Price,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };

    public static OrderChangeMessage ToChangeMessage(this Order order) =>
        new(order.Id, order.ProductId, order.UserId, order.Price, order.Status, order.CreatedAt);

    public static Order ToModel(this OrderChangeMessage message) =>
        new(message.Id, message.ProductId, message.UserId, message.Price, message.Status, message.UpdatedAt);
}

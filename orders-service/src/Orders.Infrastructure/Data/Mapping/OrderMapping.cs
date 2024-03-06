using Orders.Domain.Models;
using Orders.Infrastructure.Data.Entities;

namespace Orders.Infrastructure.Data.Mapping;

public static class OrderMapping
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
            Quantity = order.Quantity,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };
}

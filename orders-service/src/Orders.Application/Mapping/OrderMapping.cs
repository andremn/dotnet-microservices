using Orders.Application.Messaging.Messages;
using Orders.Domain.Dtos;
using Orders.Domain.Entities;
using Orders.Domain.Enums;

namespace Orders.Application.Mapping;

public static class OrderMapping
{
    public static OrderDto ToDto(this Order entity) =>
        new(entity.Id, entity.ProductId, entity.UserId, entity.ProductSnapshot.ToDto(), entity.Quantity, entity.Status, entity.CreatedAt);

    public static Order ToEntity(this OrderDto dto) =>
        new()
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            ProductSnapshot = dto.ProductSnapshot.ToEntity(),
            UserId = dto.UserId,
            Quantity = dto.Quantity,
            Status = dto.Status,
            CreatedAt = dto.CreatedAt
        };

    public static OrderCreatedMessage ToOrderCreatedMessage(this OrderDto dto) =>
        new(dto.Id, dto.ProductId, dto.UserId, dto.ProductSnapshot, dto.Quantity, dto.CreatedAt);

    public static OrderPaymentRequestMessage ToPaymentRequestMessage(this OrderDto dto) =>
        new(dto.Id, dto.ProductSnapshot.Price);

    public static OrderShippingRequestMessage ToOrderShippingRequestMessage(this OrderDto dto) =>
        new(dto.Id);

    public static OrderDto ToDto(this OrderCreatedMessage message) =>
        new(
            message.Id,
            message.ProductId,
            message.UserId,
            message.ProductSnapshot,
            message.Quantity,
            OrderStatus.Created,
            message.CreatedAt);
}

using Orders.Domain.Enums;

namespace Orders.Domain.Dtos;

public record OrderDto(int Id, int ProductId, string UserId, ProductSnapshotDto ProductSnapshot, int Quantity, OrderStatus Status, DateTime CreatedAt);

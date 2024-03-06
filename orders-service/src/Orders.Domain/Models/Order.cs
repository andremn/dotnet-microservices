using Orders.Domain.Enums;

namespace Orders.Domain.Models;

public record Order(int Id, int ProductId, string UserId, ProductSnapshot ProductSnapshot, int Quantity, OrderStatus Status, DateTime CreatedAt);

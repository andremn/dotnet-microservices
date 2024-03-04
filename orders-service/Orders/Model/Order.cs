namespace Orders.Model;

public record Order(int Id, int ProductId, string UserId, ProductSnapshot ProductSnapshot, int Quantity, OrderStatus Status, DateTime CreatedAt);

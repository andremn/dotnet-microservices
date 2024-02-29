namespace Orders.Model;

public record Order(int Id, int ProductId, string UserId, decimal Price, OrderStatus Status, DateTime CreatedAt);

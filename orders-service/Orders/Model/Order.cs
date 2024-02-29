namespace Orders.Model;

public record Order(int Id, int ProductId, string UserId, decimal Price, DateTime CreatedAt);

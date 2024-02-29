namespace Orders.Model;

public record DetailedOrder(int Id, Product Product, DateTime CreatedAt);

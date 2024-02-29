using Orders.Model;

namespace Orders.Repositories.Entities;

public class OrderEntity
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

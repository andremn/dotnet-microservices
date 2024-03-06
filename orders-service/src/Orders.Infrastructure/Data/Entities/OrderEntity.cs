using Orders.Domain.Enums;

namespace Orders.Infrastructure.Data.Entities;

public class OrderEntity
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ProductSnapshotId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ProductSnapshotEntity ProductSnapshot { get; set; } = null!;
}

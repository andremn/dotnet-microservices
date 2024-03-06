namespace Orders.Infrastructure.Data.Entities;

public class ProductSnapshotEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public OrderEntity? Order { get; set; }
}

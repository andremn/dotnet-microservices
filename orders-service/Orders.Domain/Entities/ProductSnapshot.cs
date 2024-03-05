namespace Orders.Domain.Entities;

public class ProductSnapshot
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public Order? Order { get; set; }
}

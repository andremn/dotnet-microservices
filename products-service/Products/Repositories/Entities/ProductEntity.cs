namespace Products.Repositories.Entities;

public class ProductEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}

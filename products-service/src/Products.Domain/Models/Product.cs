namespace Products.Domain.Models;

public record Product(int Id, string Name, string Description, int Quantity, decimal Price);

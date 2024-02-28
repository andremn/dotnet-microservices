namespace Products.Controllers.Products;

public record CreateProductRequest(string Name, string Description, int Quantity, decimal Price);

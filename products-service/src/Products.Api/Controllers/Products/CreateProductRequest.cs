namespace Products.Api.Controllers.Products;

public record CreateProductRequest(string Name, string Description, int Quantity, decimal Price);

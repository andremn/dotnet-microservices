namespace Products.Controllers.Products;

public record UpdateProductRequest(int Id, string Name, string Description, int Quantity, decimal Price);

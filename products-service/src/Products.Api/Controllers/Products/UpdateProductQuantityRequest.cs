namespace Products.Api.Controllers.Products;

public record UpdateProductQuantityRequest(int Quantity, string Operation);

public enum UpdateProductQuantityOperation
{
    Increment = 0,
    Decrement
}

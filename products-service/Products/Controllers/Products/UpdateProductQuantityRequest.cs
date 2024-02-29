namespace Products.Controllers.Products;

public record UpdateProductQuantityRequest(int Quantity, UpdateProductQuantityOperation Operation);

public enum UpdateProductQuantityOperation
{
    Increment = 0,
    Decrement
}

namespace Orders.Services.Results;

public enum ResultErrorReason
{
    None = 0,
    Validation = 1,
    OrderNotFound = 2,
    ProductNotFound = 3,
    CannotUpdateProduct = 4
}

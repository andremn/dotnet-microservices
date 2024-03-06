namespace Orders.Application.Enums;

public enum ResultErrorReason
{
    None = 0,
    Validation = 1,
    OrderNotFound = 2,
    ProductNotFound = 3,
    CannotUpdateProduct = 4,
    InvalidProductQuantity = 5
}

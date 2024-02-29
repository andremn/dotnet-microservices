namespace Orders.Services.Results;

public record UpdateOrderResult(bool IsSuccess, int Id, ResultErrorReason ErrorReason)
{
    public static UpdateOrderResult Success(int id) =>
        new(true, id, ResultErrorReason.None);

    public static UpdateOrderResult NotFound() =>
        new(false, 0, ResultErrorReason.OrderNotFound);

    public static UpdateOrderResult CannotUpdateProduct() =>
        new(false, 0, ResultErrorReason.CannotUpdateProduct);
}

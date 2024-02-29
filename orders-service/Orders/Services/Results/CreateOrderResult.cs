namespace Orders.Services.Results;

public record CreateOrderResult(bool Success, int Id, ResultErrorReason ErrorReason)
{
    public static CreateOrderResult FromSuccess(int id) =>
        new(true, id, ResultErrorReason.None);

    public static CreateOrderResult FromNotFoundError() =>
        new(false, 0, ResultErrorReason.ProductNotFound);
}

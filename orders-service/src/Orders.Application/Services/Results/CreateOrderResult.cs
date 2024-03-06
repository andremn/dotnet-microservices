using Orders.Application.Enums;

namespace Orders.Application.Services.Results;

public record CreateOrderResult(bool IsSuccess, int Id, ResultErrorReason ErrorReason)
{
    public static CreateOrderResult Success(int id) =>
        new(true, id, ResultErrorReason.None);

    public static CreateOrderResult InvalidProductQuantity() =>
        new(false, 0, ResultErrorReason.InvalidProductQuantity);

    public static CreateOrderResult ProductNotFound() =>
        new(false, 0, ResultErrorReason.ProductNotFound);
}

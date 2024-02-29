﻿namespace Orders.Services.Results;

public record CreateOrderResult(bool IsSuccess, int Id, ResultErrorReason ErrorReason)
{
    public static CreateOrderResult Success(int id) =>
        new(true, id, ResultErrorReason.None);

    public static CreateOrderResult ProductNotFound() =>
        new(false, 0, ResultErrorReason.ProductNotFound);
}

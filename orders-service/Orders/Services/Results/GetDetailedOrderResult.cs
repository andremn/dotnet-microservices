using Orders.Model;

namespace Orders.Services.Results;

public record GetDetailedOrderResult(DetailedOrder? Order, ResultErrorReason ErrorReason)
{
    public static GetDetailedOrderResult CreateAsSuccess(DetailedOrder order) =>
        new(order, ResultErrorReason.None);

    public static GetDetailedOrderResult NotFound() =>
        new(null, ResultErrorReason.OrderNotFound);

    public static GetDetailedOrderResult ProductNotFound() =>
        new(null, ResultErrorReason.ProductNotFound);
}

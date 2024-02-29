using Orders.Model;

namespace Orders.Services.Results;

public record GetDetailedOrderResult(DetailedOrder? Order, ResultErrorReason ErrorReason)
{
    public static GetDetailedOrderResult FromSuccess(DetailedOrder order) =>
        new(order, ResultErrorReason.None);

    public static GetDetailedOrderResult FromNotFoundError() =>
        new(null, ResultErrorReason.NotFound);

    public static GetDetailedOrderResult FromProductNotFoundError() =>
        new(null, ResultErrorReason.ProductNotFound);
}

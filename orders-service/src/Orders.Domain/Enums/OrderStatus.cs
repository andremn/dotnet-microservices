namespace Orders.Domain.Enums;

public enum OrderStatus
{
    Created = 0,
    AwaitingPayment = 1,
    PaymentConfirmed = 2,
    PaymentDenied = 3,
    AwaitingShipping = 4,
    Shipped = 5,
    DeliveryFailed = 6,
    Finished = 7,
    ProductNotAvailable = 8,
    Canceled = 9
}

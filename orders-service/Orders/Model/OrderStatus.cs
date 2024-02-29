namespace Orders.Model;

public enum OrderStatus
{
    Created = 0,
    AwaitingPayment = 1,
    PaymentConfirmed = 2,
    AwaitingShipping = 3,
    Shipped = 4,
    Finished = 5
}

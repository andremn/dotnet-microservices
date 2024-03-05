namespace Orders.Application.Enums;

public enum OrderShippingStatus
{
    AwaitingCollect = 0,
    Collected = 1,
    EnRoute = 2,
    Delivered = 3,
    NotDelivered = 4
}

using Orders.Application.Enums;

namespace Orders.Application.Messaging.Messages;

public record OrderShippingStatusChangedMessage(int Id, OrderShippingStatus Status);

using Orders.Model;

namespace Orders.Messaging.Messages;

public record OrderShippingStatusChangedMessage(int Id, OrderShippingStatus Status);

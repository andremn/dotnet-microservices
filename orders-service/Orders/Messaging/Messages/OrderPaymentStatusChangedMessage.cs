using Orders.Model;

namespace Orders.Messaging.Messages;

public record OrderPaymentStatusChangedMessage(int Id, OrderPaymentStatus Status);

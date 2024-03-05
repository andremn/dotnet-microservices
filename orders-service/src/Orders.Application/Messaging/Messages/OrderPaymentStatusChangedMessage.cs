using Orders.Application.Enums;

namespace Orders.Application.Messaging.Messages;

public record OrderPaymentStatusChangedMessage(int Id, OrderPaymentStatus Status);

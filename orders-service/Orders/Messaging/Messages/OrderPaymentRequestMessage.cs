using Orders.Model;

namespace Orders.Messaging.Messages;

public record OrderPaymentRequestMessage(int Id, decimal Price);

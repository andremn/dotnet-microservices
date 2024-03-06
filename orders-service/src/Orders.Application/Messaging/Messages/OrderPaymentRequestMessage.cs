namespace Orders.Application.Messaging.Messages;

public record OrderPaymentRequestMessage(int Id, decimal Price);

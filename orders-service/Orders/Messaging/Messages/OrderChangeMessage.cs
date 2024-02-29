using Orders.Model;

namespace Orders.Messaging.Messages;

public record OrderChangeMessage(int Id, int ProductId, string UserId, decimal Price, int Quantity, OrderStatus Status, DateTime UpdatedAt);

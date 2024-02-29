using Orders.Model;

namespace Orders.Messaging.Messages;

public record OrderChangeMessage(int Id, int ProductId, string UserId, decimal Price, OrderStatus Status, DateTime UpdatedAt);

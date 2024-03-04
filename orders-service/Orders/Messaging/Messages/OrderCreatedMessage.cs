using Orders.Model;

namespace Orders.Messaging.Messages;

public record OrderCreatedMessage(int Id, int ProductId, string UserId, ProductSnapshot ProductSnapshot, int Quantity, DateTime CreatedAt);

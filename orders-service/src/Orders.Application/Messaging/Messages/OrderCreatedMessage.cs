using Orders.Domain.Models;

namespace Orders.Application.Messaging.Messages;

public record OrderCreatedMessage(int Id, int ProductId, string UserId, ProductSnapshot ProductSnapshot, int Quantity, DateTime CreatedAt);

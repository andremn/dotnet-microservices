using Orders.Domain.Dtos;

namespace Orders.Application.Messaging.Messages;

public record OrderCreatedMessage(int Id, int ProductId, string UserId, ProductSnapshotDto ProductSnapshot, int Quantity, DateTime CreatedAt);

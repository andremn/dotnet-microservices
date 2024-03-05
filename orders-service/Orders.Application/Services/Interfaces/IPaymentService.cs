using Orders.Domain.Dtos;

namespace Orders.Application.Services.Interfaces;

public interface IPaymentService
{
    Task SendApprovalRequestAsync(OrderDto order);

    Task HandlePaymentRequest(int orderId, decimal price);
}

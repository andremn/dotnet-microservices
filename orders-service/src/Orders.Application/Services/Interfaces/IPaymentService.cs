using Orders.Domain.Models;

namespace Orders.Application.Services.Interfaces;

public interface IPaymentService
{
    Task SendApprovalRequestAsync(Order order);

    Task HandlePaymentRequest(int orderId, decimal price);
}

using Orders.Model;

namespace Orders.Services.External;

public interface IPaymentService
{
    Task SendApprovalRequestAsync(Order order);

    Task HandlePaymentRequest(int orderId, decimal price);
}

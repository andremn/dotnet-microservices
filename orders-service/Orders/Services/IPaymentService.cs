using Orders.Model;

namespace Orders.Services;

public interface IPaymentService
{
    Task SendApprovalRequestAsync(Order order);
}

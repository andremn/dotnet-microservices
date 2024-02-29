using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;

namespace Orders.Services;

public class PaymentService : IPaymentService
{
    public Task SendApprovalRequestAsync(Order order)
    {
        // Fake payment service implementation simulating the approval request was sent
        return Task.CompletedTask;
    }
}

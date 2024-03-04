using Orders.Messaging.Messages;
using Orders.Services.External;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderPaymentRequestListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderPaymentRequestMessage>
{
    public async Task OnMessageReceived(OrderPaymentRequestMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var paymentService = serviceScope.ServiceProvider.GetRequiredService<IPaymentService>();

        await paymentService.HandlePaymentRequest(message.Id, message.Price);
    }
}

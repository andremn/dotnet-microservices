using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Messaging.Messages;
using Orders.Application.Services.Interfaces;

namespace Orders.Application.Messaging.Listeners;

public class OrderPaymentRequestListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderPaymentRequestMessage>
{
    public async Task OnMessageReceived(OrderPaymentRequestMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var paymentService = serviceScope.ServiceProvider.GetRequiredService<IPaymentService>();

        await paymentService.HandlePaymentRequest(message.Id, message.Price);
    }
}

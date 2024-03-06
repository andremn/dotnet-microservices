using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Messaging.Messages;
using Orders.Application.Services.Interfaces;

namespace Orders.Application.Messaging.Listeners;

public class OrderPaymentStatusChangedListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderPaymentStatusChangedMessage>
{
    public async Task OnMessageReceived(OrderPaymentStatusChangedMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var orderProcessingService = serviceScope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        await orderProcessingService.HandlePaymentStatusChangedAsync(message.Id, message.Status);
    }
}

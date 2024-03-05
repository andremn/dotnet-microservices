using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Messaging.Messages;
using Orders.Application.Services.Interfaces;

namespace Orders.Application.Messaging.Listeners;

public class OrderShippingStatusChangedListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderShippingStatusChangedMessage>
{
    public async Task OnMessageReceived(OrderShippingStatusChangedMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var orderProcessingService = serviceScope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        await orderProcessingService.HandleShippingStatusChangedAsync(message.Id, message.Status);
    }
}

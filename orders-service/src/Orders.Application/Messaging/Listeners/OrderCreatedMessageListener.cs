using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Mapping;
using Orders.Application.Messaging.Messages;
using Orders.Application.Services.Interfaces;

namespace Orders.Application.Messaging.Listeners;

public class OrderCreatedMessageListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderCreatedMessage>
{
    public async Task OnMessageReceived(OrderCreatedMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var orderProcessingService = serviceScope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        await orderProcessingService.HandleOrderCreatedAsync(message.ToDto());
    }
}

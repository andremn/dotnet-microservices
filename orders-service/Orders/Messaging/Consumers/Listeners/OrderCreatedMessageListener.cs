using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Services;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderCreatedMessageListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderCreatedMessage>
{
    public async Task OnMessageReceived(OrderCreatedMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var orderProcessingService = serviceScope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        await orderProcessingService.HandleOrderCreatedAsync(message.ToModel());
    }
}

using Orders.Messaging.Messages;
using Orders.Services.Orders;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderShippingStatusChangedListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderShippingStatusChangedMessage>
{
    public async Task OnMessageReceived(OrderShippingStatusChangedMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var orderProcessingService = serviceScope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        await orderProcessingService.HandleShippingStatusChangedAsync(message.Id, message.Status);
    }
}

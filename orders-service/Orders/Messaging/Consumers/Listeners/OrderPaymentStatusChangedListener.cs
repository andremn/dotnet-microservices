using Orders.Messaging.Messages;
using Orders.Services;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderPaymentStatusChangedListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderPaymentStatusChangedMessage>
{
    public async Task OnMessageReceived(OrderPaymentStatusChangedMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var orderProcessingService = serviceScope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

        await orderProcessingService.HandlePaymentStatusChangedAsync(message.Id, message.Status);
    }
}

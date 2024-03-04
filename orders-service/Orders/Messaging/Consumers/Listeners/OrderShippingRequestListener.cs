using Orders.Messaging.Messages;
using Orders.Services;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderShippingRequestListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderShippingRequestMessage>
{
    public async Task OnMessageReceived(OrderShippingRequestMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var shippingService = serviceScope.ServiceProvider.GetRequiredService<IShippingService>();

        await shippingService.HandleShippingRequestAsync(message.Id);
    }
}

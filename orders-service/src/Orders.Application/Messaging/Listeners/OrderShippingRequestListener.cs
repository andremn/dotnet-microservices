using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Messaging.Messages;
using Orders.Application.Services.Interfaces;

namespace Orders.Application.Messaging.Listeners;

public class OrderShippingRequestListener(IServiceScopeFactory serviceScopeFactory) : IListener<OrderShippingRequestMessage>
{
    public async Task OnMessageReceived(OrderShippingRequestMessage message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var shippingService = serviceScope.ServiceProvider.GetRequiredService<IShippingService>();

        await shippingService.HandleShippingRequestAsync(message.Id);
    }
}

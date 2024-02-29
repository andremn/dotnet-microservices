using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Services;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderChangeMessageListener(IOrderProcessingService orderProcessingService) : IListener<OrderChangeMessage>
{
    public async Task OnMessageReceived(OrderChangeMessage message)
    {
        await orderProcessingService.HandleOrderChangedAsync(message.ToModel());
    }
}

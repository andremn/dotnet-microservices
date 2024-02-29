using Orders.Messaging.Messages;
using Orders.Services;

namespace Orders.Messaging.Consumers.Listeners;

public class OrderChangeMessageListener(IOrderService orderService) : IListener<OrderChangeMessage>
{
    public async Task OnMessageReceived(OrderChangeMessage message)
    {
        await orderService.CreateAsync(0);
    }
}

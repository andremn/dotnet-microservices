namespace Orders.Application.Messaging.Listeners;

public interface IListener<TMessage>
{
    Task OnMessageReceived(TMessage message);
}

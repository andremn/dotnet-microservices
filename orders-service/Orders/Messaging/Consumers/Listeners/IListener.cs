namespace Orders.Messaging.Consumers.Listeners;

public interface IListener<TMessage>
{
    Task OnMessageReceived(TMessage message);
}

namespace Orders.Application.Messaging.Publishers;

public interface IPublisher<TMessage>
{
    void Publish(TMessage message);
}

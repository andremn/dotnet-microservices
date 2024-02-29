namespace Orders.Messaging.Producers.Publishers;

public interface IPublisher<TMessage>
{
    void Publish(TMessage message);
}

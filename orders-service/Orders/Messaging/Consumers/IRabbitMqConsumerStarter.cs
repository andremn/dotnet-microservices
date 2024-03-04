namespace Orders.Messaging.Consumers;

public interface IRabbitMqConsumerStarter : IDisposable
{
    void StartReceivingMessages();
}
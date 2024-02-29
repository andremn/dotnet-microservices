namespace Orders.Messaging.Consumers;

public interface IRabbitMqConsumerService : IDisposable
{
    void StartReceivingMessages();
}
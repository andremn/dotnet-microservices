namespace Orders.Application.Messaging;

public interface IRabbitMqConsumerStarter : IDisposable
{
    void StartReceivingMessages();
}
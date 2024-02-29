namespace Orders.Messaging.Producers;

public interface IRabbitMqProducerService
{
    void SendMessage<T>(T message, string exchange, string routingKey);
}
namespace Orders.Application.Messaging;

public interface IRabbitMqProducerService
{
    void SendMessage<T>(T message, string exchange, string routingKey);
}
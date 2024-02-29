using RabbitMQ.Client;

namespace Orders.Messaging;
public interface IRabbitMqService
{
    IConnection CreateConnection();
}
using RabbitMQ.Client;

namespace Orders.Application.Messaging;

public interface IRabbitMqService
{
    IConnection CreateConnection();
}
using Orders.Application.Messaging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Orders.Infrastructure.RabbitMq.Producers;

public class RabbitMqProducerService : IRabbitMqProducerService
{
    private readonly IModel _model;

    public RabbitMqProducerService(IRabbitMqService rabbitMqService)
    {
        var connection = rabbitMqService.CreateConnection();

        _model = connection.CreateModel();
    }

    public void SendMessage<T>(T message, string exchange, string routingKey)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _model.BasicPublish(
            exchange: exchange,
            routingKey: routingKey,
            body: body);
    }
}

using Microsoft.Extensions.Options;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Configurations;
using RabbitMQ.Client;

namespace Orders.Infrastructure.RabbitMq;

public class RabbitMqService(IOptions<RabbitMqConfiguration> options) : IRabbitMqService
{
    private readonly RabbitMqConfiguration _rabbitMqConfig = options.Value;

    public IConnection CreateConnection()
    {
        var connection = new ConnectionFactory
        {
            UserName = _rabbitMqConfig.UserName,
            Password = _rabbitMqConfig.Password,
            HostName = _rabbitMqConfig.HostName,
            DispatchConsumersAsync = true
        };

        return connection.CreateConnection();
    }
}

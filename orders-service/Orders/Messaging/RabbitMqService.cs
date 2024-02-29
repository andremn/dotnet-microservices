using Microsoft.Extensions.Options;
using Orders.Configurations;
using RabbitMQ.Client;

namespace Orders.Messaging;

public class RabbitMqService(IOptions<RabbitMqConfig> options) : IRabbitMqService
{
    private readonly RabbitMqConfig _rabbitMqConfig = options.Value;

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

using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using RabbitMQ.Client;
using System.Text.Json;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public class RabbitMqOrdersConsumerService(
    IServiceScopeFactory serviceScopeFactory,
    IRabbitMqService rabbitMqService,
    IOptions<RabbitMqConfig> options,
    ILogger<RabbitMqOrdersConsumerService> logger
) : RabbitMqConsumerService(rabbitMqService, options, logger)
{
    protected override RabbitMqConsumerConfig ConsumerConfig => Config.OrderChangeConsumer;

    protected override IModel CreateModel(IConnection connection)
    {
        var model = connection.CreateModel();

        model.QueueDeclare(ConsumerConfig.Queue, durable: true, exclusive: false, autoDelete: false);
        model.ExchangeDeclare(ConsumerConfig.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);

        model.QueueBind(ConsumerConfig.Queue, ConsumerConfig.Exchange, ConsumerConfig.RoutingKey);

        return model;
    }

    protected override async Task ProcessAsync(string message)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();
        var listeners = serviceScope.ServiceProvider.GetServices<IListener<OrderChangeMessage>>();
        var orderChangeMessage = JsonSerializer.Deserialize<OrderChangeMessage>(message);

        if (orderChangeMessage is null)
        {
            logger.LogWarning("Could not deserialize message {message}", message);
            return;
        }

        foreach (var listener in listeners)
        {
            await listener.OnMessageReceived(orderChangeMessage);
        }
    }

}

using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public class RabbitMqOrderCreatedConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderCreatedMessage> listener,
    IOptions<RabbitMqConfig> options,
    ILogger<RabbitMqOrderCreatedConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderCreatedMessage>(rabbitMqService, listener, options, logger)
{
    protected override RabbitMqMessageConfig MessageConfig => Config.OrderCreated;
}

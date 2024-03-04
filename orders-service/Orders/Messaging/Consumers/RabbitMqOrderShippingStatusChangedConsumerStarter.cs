using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public class RabbitMqOrderShippingStatusChangedConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderShippingStatusChangedMessage> listener,
    IOptions<RabbitMqConfig> options,
    ILogger<RabbitMqOrderShippingStatusChangedConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderShippingStatusChangedMessage>(rabbitMqService, listener, options, logger)
{
    protected override RabbitMqMessageConfig MessageConfig => Config.OrderShippingStatusChanged;
}

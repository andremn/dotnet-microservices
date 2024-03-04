using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public class RabbitMqOrderShippingRequestConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderShippingRequestMessage> listener,
    IOptions<RabbitMqConfig> options,
    ILogger<RabbitMqOrderShippingRequestConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderShippingRequestMessage>(rabbitMqService, listener, options, logger)
{
    protected override RabbitMqMessageConfig MessageConfig => Config.OrderShippingRequest;
}

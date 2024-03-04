using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public class RabbitMqOrderPaymentRequestConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderPaymentRequestMessage> listener,
    IOptions<RabbitMqConfig> options,
    ILogger<RabbitMqOrderPaymentRequestConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderPaymentRequestMessage>(rabbitMqService, listener, options, logger)
{
    protected override RabbitMqMessageConfig MessageConfig => Config.OrderPaymentRequest;
}

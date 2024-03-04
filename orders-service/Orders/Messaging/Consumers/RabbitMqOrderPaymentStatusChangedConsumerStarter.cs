using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public class RabbitMqOrderPaymentStatusChangedConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderPaymentStatusChangedMessage> listener,
    IOptions<RabbitMqConfig> options,
    ILogger<RabbitMqOrderPaymentStatusChangedConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderPaymentStatusChangedMessage>(rabbitMqService, listener, options, logger)
{
    protected override RabbitMqMessageConfig MessageConfig => Config.OrderPaymentStatusChanged;
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Listeners;
using Orders.Application.Messaging.Messages;

namespace Orders.Infrastructure.RabbitMq.Consumers.Orders;

public class RabbitMqOrderShippingStatusChangedConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderShippingStatusChangedMessage> listener,
    IOptions<RabbitMqConfiguration> options,
    ILogger<RabbitMqOrderShippingStatusChangedConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderShippingStatusChangedMessage>(rabbitMqService, listener, options, logger)
{
    protected override string ClientProfileKey => "OrderShippingStatusChanged";
}

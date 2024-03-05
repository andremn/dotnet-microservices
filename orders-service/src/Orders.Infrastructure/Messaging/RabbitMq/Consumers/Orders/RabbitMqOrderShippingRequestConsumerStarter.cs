using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Listeners;
using Orders.Application.Messaging.Messages;

namespace Orders.Infrastructure.RabbitMq.Consumers.Orders;

public class RabbitMqOrderShippingRequestConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderShippingRequestMessage> listener,
    IOptions<RabbitMqConfiguration> options,
    ILogger<RabbitMqOrderShippingRequestConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderShippingRequestMessage>(rabbitMqService, listener, options, logger)
{
    protected override RabbitMqMessageConfiguration MessageConfiguration => Configuration.OrderShippingRequest;
}

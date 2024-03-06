using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Listeners;
using Orders.Application.Messaging.Messages;

namespace Orders.Infrastructure.RabbitMq.Consumers.Orders;

public class RabbitMqOrderCreatedConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderCreatedMessage> listener,
    IOptions<RabbitMqConfiguration> options,
    ILogger<RabbitMqOrderCreatedConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderCreatedMessage>(rabbitMqService, listener, options, logger)
{
    protected override string ClientProfileKey => "OrderCreated";
}

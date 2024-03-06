using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Listeners;
using Orders.Application.Messaging.Messages;

namespace Orders.Infrastructure.RabbitMq.Consumers.Orders;

public class RabbitMqOrderPaymentRequestConsumerStarter(
    IRabbitMqService rabbitMqService,
    IListener<OrderPaymentRequestMessage> listener,
    IOptions<RabbitMqConfiguration> options,
    ILogger<RabbitMqOrderPaymentRequestConsumerStarter> logger
) : RabbitMqBaseConsumerStarter<OrderPaymentRequestMessage>(rabbitMqService, listener, options, logger)
{
    protected override string ClientProfileKey => "OrderPaymentRequest";
}

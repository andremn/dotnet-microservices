using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderPaymentStatusChangedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderPaymentStatusChangedMessage>(rabbitMqProducerService, options), IPublisher<OrderPaymentStatusChangedMessage>
{
    protected override string ClientProfileKey => "OrderPaymentStatusChanged";
}

using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderPaymentStatusChangedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderPaymentStatusChangedMessage>(rabbitMqProducerService), IPublisher<OrderPaymentStatusChangedMessage>
{
    protected override RabbitMqMessageConfiguration MessageConfiguration => options.Value.OrderPaymentStatusChanged;
}

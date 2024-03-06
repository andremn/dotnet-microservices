using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderShippingStatusChangedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderShippingStatusChangedMessage>(rabbitMqProducerService), IPublisher<OrderShippingStatusChangedMessage>
{
    protected override RabbitMqMessageConfiguration MessageConfiguration => options.Value.OrderShippingStatusChanged;
}

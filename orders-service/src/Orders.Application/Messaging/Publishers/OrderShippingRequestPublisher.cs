using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderShippingRequestPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderShippingRequestMessage>(rabbitMqProducerService), IPublisher<OrderShippingRequestMessage>
{
    protected override RabbitMqMessageConfiguration MessageConfiguration => options.Value.OrderShippingRequest;
}

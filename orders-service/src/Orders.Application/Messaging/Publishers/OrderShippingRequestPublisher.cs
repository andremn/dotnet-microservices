using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderShippingRequestPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderShippingRequestMessage>(rabbitMqProducerService, options), IPublisher<OrderShippingRequestMessage>
{
    protected override string ClientProfileKey => "OrderShippingRequest";
}

using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderCreatedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderCreatedMessage>(rabbitMqProducerService), IPublisher<OrderCreatedMessage>
{
    protected override RabbitMqMessageConfiguration MessageConfiguration => options.Value.OrderCreated;
}

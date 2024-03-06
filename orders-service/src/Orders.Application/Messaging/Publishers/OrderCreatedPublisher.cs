using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderCreatedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderCreatedMessage>(rabbitMqProducerService, options), IPublisher<OrderCreatedMessage>
{
    protected override string ClientProfileKey => "OrderCreated";
}

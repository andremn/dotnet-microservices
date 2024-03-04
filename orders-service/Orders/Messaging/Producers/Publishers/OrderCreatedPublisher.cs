using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public class OrderCreatedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfig> options
) : RabbitMqPublisher<OrderCreatedMessage>(rabbitMqProducerService), IPublisher<OrderCreatedMessage>
{
    protected override RabbitMqMessageConfig MessageConfig => options.Value.OrderCreated;
}

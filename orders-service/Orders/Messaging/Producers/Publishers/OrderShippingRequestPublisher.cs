using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public class OrderShippingRequestPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfig> options
) : RabbitMqPublisher<OrderShippingRequestMessage>(rabbitMqProducerService), IPublisher<OrderShippingRequestMessage>
{    protected override RabbitMqMessageConfig MessageConfig => options.Value.OrderShippingRequest;
}

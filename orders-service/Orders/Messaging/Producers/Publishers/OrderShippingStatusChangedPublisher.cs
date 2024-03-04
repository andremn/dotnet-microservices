using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public class OrderShippingStatusChangedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfig> options
) : RabbitMqPublisher<OrderShippingStatusChangedMessage>(rabbitMqProducerService), IPublisher<OrderShippingStatusChangedMessage>
{    protected override RabbitMqMessageConfig MessageConfig => options.Value.OrderShippingStatusChanged;
}

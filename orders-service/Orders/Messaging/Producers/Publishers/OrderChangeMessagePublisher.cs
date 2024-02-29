using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public class OrderChangeMessagePublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfig> options
) : IPublisher<OrderChangeMessage>
{
    private readonly RabbitMqProducerConfig _producerConfig = options.Value.OrderChangeProducer;

    public void Publish(OrderChangeMessage message)
    {
        rabbitMqProducerService.SendMessage(message, _producerConfig.Exchange, _producerConfig.RoutingKey);
    }
}

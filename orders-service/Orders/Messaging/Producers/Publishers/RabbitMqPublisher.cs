using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public abstract class RabbitMqPublisher<TMessage>(IRabbitMqProducerService rabbitMqProducerService) : IPublisher<TMessage>
{
    protected abstract RabbitMqMessageConfig MessageConfig { get; }

    public void Publish(TMessage message)
    {
        rabbitMqProducerService.SendMessage(message, MessageConfig.Exchange, MessageConfig.RoutingKey);
    }
}

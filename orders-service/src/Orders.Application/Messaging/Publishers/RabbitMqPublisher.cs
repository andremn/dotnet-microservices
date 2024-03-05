using Orders.Application.Messaging.Configurations;

namespace Orders.Application.Messaging.Publishers;

public abstract class RabbitMqPublisher<TMessage>(IRabbitMqProducerService rabbitMqProducerService) : IPublisher<TMessage>
{
    protected abstract RabbitMqMessageConfiguration MessageConfiguration { get; }

    public void Publish(TMessage message)
    {
        rabbitMqProducerService.SendMessage(message, MessageConfiguration.Exchange, MessageConfiguration.RoutingKey);
    }
}

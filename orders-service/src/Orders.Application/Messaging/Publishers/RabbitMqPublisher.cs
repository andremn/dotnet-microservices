using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;

namespace Orders.Application.Messaging.Publishers;

public abstract class RabbitMqPublisher<TMessage> : IPublisher<TMessage>
{
    private readonly RabbitMqClientProfile _clientProfile;
    private readonly IRabbitMqProducerService _rabbitMqProducerService;

    protected RabbitMqPublisher(IRabbitMqProducerService rabbitMqProducerService, IOptions<RabbitMqConfiguration> options)
    {
        _clientProfile = options.Value.ClientProfiles.SingleOrDefault(x => x.Key == ClientProfileKey) ??
            throw new ArgumentException($"Cannot find client profile with key '{ClientProfileKey}'");
        _rabbitMqProducerService = rabbitMqProducerService;
    }

    protected abstract string ClientProfileKey { get; }

    public void Publish(TMessage message)
    {
        _rabbitMqProducerService.SendMessage(message, _clientProfile.Exchange, _clientProfile.RoutingKey);
    }
}

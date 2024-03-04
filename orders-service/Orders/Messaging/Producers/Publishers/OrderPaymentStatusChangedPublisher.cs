using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public class OrderPaymentStatusChangedPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfig> options
) : RabbitMqPublisher<OrderPaymentStatusChangedMessage>(rabbitMqProducerService), IPublisher<OrderPaymentStatusChangedMessage>
{    protected override RabbitMqMessageConfig MessageConfig => options.Value.OrderPaymentStatusChanged;
}

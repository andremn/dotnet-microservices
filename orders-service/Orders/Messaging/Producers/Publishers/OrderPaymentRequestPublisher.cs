using Microsoft.Extensions.Options;
using Orders.Configurations;
using Orders.Messaging.Messages;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Producers.Publishers;

public class OrderPaymentRequestPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfig> options
) : RabbitMqPublisher<OrderPaymentRequestMessage>(rabbitMqProducerService), IPublisher<OrderPaymentRequestMessage>
{    protected override RabbitMqMessageConfig MessageConfig => options.Value.OrderPaymentRequest;
}

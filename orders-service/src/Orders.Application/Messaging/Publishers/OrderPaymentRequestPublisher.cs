using Microsoft.Extensions.Options;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Messages;

namespace Orders.Application.Messaging.Publishers;

public class OrderPaymentRequestPublisher(
    IRabbitMqProducerService rabbitMqProducerService,
    IOptions<RabbitMqConfiguration> options
) : RabbitMqPublisher<OrderPaymentRequestMessage>(rabbitMqProducerService), IPublisher<OrderPaymentRequestMessage>
{
    protected override RabbitMqMessageConfiguration MessageConfiguration => options.Value.OrderPaymentRequest;
}

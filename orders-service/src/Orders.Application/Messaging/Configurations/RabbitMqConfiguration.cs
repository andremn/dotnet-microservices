namespace Orders.Application.Messaging.Configurations;

public record RabbitMqConfiguration
{
    public string HostName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public RabbitMqMessageConfiguration OrderCreated { get; set; } = new RabbitMqMessageConfiguration();

    public RabbitMqMessageConfiguration OrderPaymentStatusChanged { get; set; } = new RabbitMqMessageConfiguration();

    public RabbitMqMessageConfiguration OrderShippingStatusChanged { get; set; } = new RabbitMqMessageConfiguration();

    public RabbitMqMessageConfiguration OrderPaymentRequest { get; set; } = new RabbitMqMessageConfiguration();

    public RabbitMqMessageConfiguration OrderShippingRequest { get; set; } = new RabbitMqMessageConfiguration();
}

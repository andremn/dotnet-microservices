namespace Orders.Configurations;

public record RabbitMqConfig
{
    public string HostName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public RabbitMqMessageConfig OrderCreated { get; set; } = new RabbitMqMessageConfig();

    public RabbitMqMessageConfig OrderPaymentStatusChanged { get; set; } = new RabbitMqMessageConfig();

    public RabbitMqMessageConfig OrderShippingStatusChanged { get; set; } = new RabbitMqMessageConfig();

    public RabbitMqMessageConfig OrderPaymentRequest { get; set; } = new RabbitMqMessageConfig();

    public RabbitMqMessageConfig OrderShippingRequest { get; set; } = new RabbitMqMessageConfig();

    public record RabbitMqMessageConfig
    {
        public string Exchange { get; set; } = string.Empty;

        public string Queue { get; set; } = string.Empty;

        public string RoutingKey { get; set; } = string.Empty;
    }
}

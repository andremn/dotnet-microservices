namespace Orders.Configurations;

public record RabbitMqConfig
{
    public string HostName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public RabbitMqConsumerConfig OrderChangeConsumer { get; set; } = new RabbitMqConsumerConfig();

    public RabbitMqProducerConfig OrderChangeProducer { get; set; } = new RabbitMqProducerConfig();

    public record RabbitMqConsumerConfig
    {
        public string Exchange { get; set; } = string.Empty;

        public string Queue { get; set; } = string.Empty;

        public string RoutingKey { get; set; } = string.Empty;
    }

    public record RabbitMqProducerConfig
    {
        public string Exchange { get; set; } = string.Empty;

        public string RoutingKey { get; set; } = string.Empty;
    }
}

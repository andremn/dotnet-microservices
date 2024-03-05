namespace Orders.Application.Messaging.Configurations;

public record RabbitMqMessageConfiguration
{
    public string Exchange { get; set; } = string.Empty;

    public string Queue { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;
}

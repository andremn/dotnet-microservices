namespace Orders.Application.Messaging.Configurations;

public record RabbitMqClientProfile
{
    public string Key { get; set; } = string.Empty;

    public string Exchange { get; set; } = string.Empty;

    public string Queue { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;
}

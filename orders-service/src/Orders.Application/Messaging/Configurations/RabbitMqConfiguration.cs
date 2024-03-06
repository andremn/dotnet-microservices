namespace Orders.Application.Messaging.Configurations;

public record RabbitMqConfiguration
{
    public string HostName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<RabbitMqClientProfile> ClientProfiles { get; set; } = [];
}

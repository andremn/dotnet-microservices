namespace Products.Config;

public record JwtConfig
{
    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public TimeSpan TimeToExpire { get; set; }
}

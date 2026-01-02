namespace Api.Configuration;

public record RedisConfiguration
{
    public required string Host { get; set; }
    
    public required string Password { get; set; }

    public required short Port { get; set; } = 6379;
    
    public required short Database { get; set; }
}
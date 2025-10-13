namespace Api.Configuration;

public class DatabaseConfiguration
{
    public required string Host { get; set; }
    
    public required string Password { get; set; }

    public required short Port { get; set; } = 5432;
    
    public required string User { get; set; }
    
    public required string Database { get; set; }
}
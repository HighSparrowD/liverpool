namespace Api.Configuration;

public class AppConfiguration
{
    public required DatabaseConfiguration Database { get; set; }
    
    public required RedisConfiguration Redis { get; set; }
}
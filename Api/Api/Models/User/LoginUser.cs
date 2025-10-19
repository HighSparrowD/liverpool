namespace Api.Models.User;

public record LoginUser
{
    public required string Password { get; set; }
    
    public required string Username { get; set; }
}
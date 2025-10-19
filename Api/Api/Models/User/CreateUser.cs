namespace Api.Models.User;

public record CreateUser
{
    public required string Password { get; set; }
    
    public string Username { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string Description { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
}
namespace Api.Models.User;

public record User
{
    public required long Id { get; set; }

    public required string Nickname { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required long Description { get; set; }

    public required short Age { get; set; }
    
    public bool Verified { get; set; }

    public byte Rating { get; set; }
    
    public required DateTime RegisteredAt { get; set; }
}
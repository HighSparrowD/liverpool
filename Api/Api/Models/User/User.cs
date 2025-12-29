namespace Api.Models.User;

public record User
{
    public long Id { get; set; }

    public string Username { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Description { get; set; }
    
    public string? ProfilePictureBase64 { get; set; }
    
    public DateOnly DateOfBirth { get; set; }
    
    public bool Verified { get; set; }

    public byte Rating { get; set; }
    
    public DateTime RegisteredAt { get; set; }
}
namespace Api.Entities.User;

public record User
{
    public required long Id { get; set; }

    public required string Nickname { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required long Description { get; set; }
    
    public DateTime? VerifiedAt { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    public required DateTime RegisteredAt { get; set; }
    
    public virtual List<UserRating>? Ratings { get; set; }
}
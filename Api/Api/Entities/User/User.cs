using Api.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace Api.Entities.User;

public record User : ILiverpoolEntity<Models.User.User>, IUser
{
    public long Id { get; set; }

    public required string Username { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string Description { get; set; }
    
    public required string? ProfilePictureBase64 { get; set; }
    
    public required string PasswordHash { get; set; }

    public required string PasswordSalt { get; set; }
    
    public DateTime? VerifiedAt { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    public required DateTime RegisteredAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public virtual List<UserRating>? Ratings { get; set; }
    
    public virtual List<Event.Event>? Events { get; set; }

    public Models.User.User ToDto()
    {
        return new Models.User.User
        {
            Id = Id,
            DateOfBirth = DateOfBirth,
            Username = Username,
            FirstName = FirstName,
            LastName = LastName,
            Description = Description,
            ProfilePictureBase64 = ProfilePictureBase64,
            RegisteredAt = RegisteredAt
        };
    }

    public void HashPassword(string password)
    {
        PasswordSalt =  Guid.NewGuid().ToString()[1..7]; 
        
        var hasher = new PasswordHasher<User>();
        PasswordHash = hasher.HashPassword(this, password + PasswordSalt);
    }

    public bool LogIn(string password)
    {
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(this, PasswordHash, password + PasswordSalt);
        
        return result == PasswordVerificationResult.Success;
    }
}
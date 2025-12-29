using System.ComponentModel.DataAnnotations;

namespace Api.Models.User;

public record UpdateUser
{
    public required string Username { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string Description { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    // If is equal to null -> password is not changed
    public string? Password { get; set; }
    
    // If is equal to null -> profile picture is removed
    [Base64String]
    public string? PhotoBase64 { get; set; }
}
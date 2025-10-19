using System.ComponentModel.DataAnnotations;

namespace Api.Models.Event;

public record Event
{
    public long Id { get; set; }
    
    public long CreatorId { get; set; }
    
    public string Title { get; set; }
    
    [Base64String]
    public string? ImageBase64 { get; set; }
    
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public User.User? Creator { get; set; }
    
    public List<Attendee>? Attendees { get; set; }
}
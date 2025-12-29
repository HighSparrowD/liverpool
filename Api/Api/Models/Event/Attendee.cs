namespace Api.Models.Event;

public record Attendee
{
    public required long Id { get; set; }
    
    public required long UserId { get; set; }
    
    public required long EventId { get; set; }

    public AttendeeStatus? Status { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    public DateTime? RejectedAt { get; set; }
    
    public User.User? User { get; set; }
    
    public Event? Event { get; set; }
}
using Api.Entities.Event;

namespace Api.Entities.User;

// Todo: maybe rate events separately and make this into a rating of a person as an attendee
public record UserRating
{
    public required long Id { get; set; }
    
    public required long UserId { get; set; }
    
    public required long AttendeeId { get; set; }
    
    public long? EventId { get; set; }
    
    public required byte Rating { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public virtual User? User { get; set; }
    
    public virtual Attendee? Attendee { get; set; }
    
    public virtual Event.Event? Event { get; set; }
}
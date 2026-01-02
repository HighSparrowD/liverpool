namespace Api.Models.Event;

public record GetAttendees
{
    public long EventId { get; set; }
    
    public AttendeeStatus AttendeeStatus { get; set; }
}
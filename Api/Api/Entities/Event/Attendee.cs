using Api.Entities.Common;

namespace Api.Entities.Event;

public record Attendee : ILiverpoolEntity<Models.Event.Attendee>
{
    public required long Id { get; set; }
    
    public required long UserId { get; set; }
    
    public required long EventId { get; set; }

    public DateTime? ApprovedAt { get; set; }
    
    public DateTime? RejectedAt { get; set; }
    
    public virtual User.User? User { get; set; }
    
    public virtual Event? Event { get; set; }
    
    public Models.Event.Attendee ToDto()
    {
        return new Models.Event.Attendee
        {
            Id = Id,
            UserId = UserId,
            EventId = EventId,
            ApprovedAt = ApprovedAt,
            RejectedAt = RejectedAt,
            User = User?.ToDto(),
            Event = Event?.ToDto()
        };
    }
}
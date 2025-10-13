namespace Api.Entities.Event;

public record Event
{
    public required long Id { get; set; }
    
    public required long CreatorId { get; set; }

    public required string Description { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public required DateTime StartDate { get; set; }
    
    public required DateTime EndDate { get; set; }

    public virtual List<EventTag>? Tags { get; set; }
}
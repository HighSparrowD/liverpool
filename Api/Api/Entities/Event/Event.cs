using Api.Entities.Common;
using Api.Models.Event;

namespace Api.Entities.Event;

public record Event : ILiverpoolEntity<Models.Event.Event>
{
    public long Id { get; set; }
    
    public required long CreatorId { get; set; }

    public required string Title { get; set; }
    
    public string? ImageBase64 { get; set; }
    
    public required string Description { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public required DateTime? UpdatedAt { get; set; }
    
    public required DateTime StartDate { get; set; }
    
    public required DateTime EndDate { get; set; }

    public virtual User.User? Creator { get; set; }
    
    public virtual List<EventTag>? Tags { get; set; }
    
    public virtual List<Attendee>? Attendees { get; set; }

    public Models.Event.Event ToDto()
    {
        return new Models.Event.Event()
        {
            Id = Id,
            CreatorId = CreatorId,
            Title = Title,
            Description = Description,
            ImageBase64 = ImageBase64,
            CreatedAt = CreatedAt,
            StartDate = StartDate,
            EndDate = EndDate,
            Creator =  Creator?.ToDto(),
            Tags = Tags?.Select(x => x.TagId).ToList()
        };
    }
    
    public EventChatPreview ToChatPreview()
    {
        return new EventChatPreview()
        {
            Id = Id,
            Title = Title
        };
    }
}
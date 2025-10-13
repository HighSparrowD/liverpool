using Api.Entities.Common;

namespace Api.Entities.Event;

public record EventTag
{
    public long TagId { get; set; }
    public long EventId { get; set; }

    public virtual Tag? Tag { get; set; }
    
    public virtual Event? Event { get; set; }
}
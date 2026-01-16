namespace Api.Models.Event;

public record UnattendModel
{
    public long UserId { get; set; }
    
    public long EventId { get; set; }
}
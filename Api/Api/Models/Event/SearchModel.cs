namespace Api.Models.Event;

public record SearchModel
{
    public long SearcherId { get; set; }
    
    public string? Title { get; set; }
    
    public string? CreatorName { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public List<long>? Tags { get; set; }
}
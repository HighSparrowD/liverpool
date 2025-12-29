using System.ComponentModel.DataAnnotations;

namespace Api.Models.Event;

public class UpdateEvent
{
    public required long EventId { get; set; }

    public required string Title { get; set; }
    
    [Base64String]
    public required string? ImageBase64 { get; set; }
    
    public required string Description { get; set; }
    
    public required DateTime StartDate { get; set; }
    
    public required DateTime EndDate { get; set; }
    
    public List<long>? Tags { get; set; }
}
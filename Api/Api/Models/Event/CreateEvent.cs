using System.ComponentModel.DataAnnotations;

namespace Api.Models.Event;

public class CreateEvent
{
    public required long CreatorId { get; set; }

    public required string Title { get; set; }
    
    [Base64String]
    public string? ImageBase64 { get; set; }
    
    public required string Description { get; set; }
    
    public required DateTime StartDate { get; set; }
    
    public required DateTime EndDate { get; set; }
}
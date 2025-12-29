using Npgsql.Replication.PgOutput;

namespace Api.Models.Event;

public record SearchModel
{
    public string? Title { get; set; }
    
    public string? CreatorName { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public List<long>? Tags { get; set; }
}
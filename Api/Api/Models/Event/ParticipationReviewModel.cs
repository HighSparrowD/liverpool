namespace Api.Models.Event;

public class ParticipationReviewModel
{
    public long UserId { get; set; }
    
    public long EventId { get; set; }

    public bool IsAccepted { get; set; }
}
using Api.Models.Event;

namespace Api.Services.Event;

public interface IAttendeeService
{
    Task<Attendee?> GetAttendee(long userId, long eventId);
    
    Task<Attendee> ApplyToAttend(ParticipationModel model);
}
using Api.Models.Event;

namespace Api.Services.Event;

public interface IEventService
{
    Task<Models.Event.Event> GetEventById(long id);
    
    Task<List<Models.Event.Event>> GetEventsByCreatorId(long creatorId);
    
    Task<Models.Event.Event> CreateEvent(CreateEvent model);
}
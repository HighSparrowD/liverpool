using Api.Models.Event;

namespace Api.Services.Event;

public interface IEventService
{
    Task<Models.Event.Event> GetEventById(long id);
    
    Task<List<Models.Event.Event>> GetEventsByCreatorId(long creatorId);
    
    Task<Models.Event.Event> CreateEvent(CreateEvent model);
    
    Task<Models.Event.Event> UpdateEvent(UpdateEvent model);
    
    Task<IEnumerable<Models.Event.Event>> GetEventsByUsername(string username);
    
    Task<IEnumerable<Models.Event.Event>> SearchEvents(SearchModel model);
    
    Task<IEnumerable<Models.Common.Tag>> GetCommonTags();
}
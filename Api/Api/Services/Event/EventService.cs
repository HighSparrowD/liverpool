using Api.Data;
using Api.Models.Event;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Event;

public class EventService(LiverpoolDbContext dbContext, TimeProvider timeProvider) : IEventService
{
    public async Task<Models.Event.Event> GetEventById(long id)
    {
        var evnt = await dbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
        
        if (evnt == null)
            return new Models.Event.Event();
        
        return evnt.ToDto();
    }

    public Task<List<Models.Event.Event>> GetEventsByCreatorId(long creatorId)
    {
        throw new NotImplementedException();
    }

    public async Task<Models.Event.Event> CreateEvent(CreateEvent model)
    {
        var evnt = new Entities.Event.Event()
        {
            CreatorId = model.CreatorId,
            Title = model.Title,
            ImageBase64 = model.ImageBase64,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            CreatedAt = timeProvider.GetUtcNow().UtcDateTime
        };

        await dbContext.AddAsync(evnt);
        await dbContext.SaveChangesAsync();
        return evnt.ToDto();
    }
}
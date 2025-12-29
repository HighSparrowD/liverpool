using Api.Data;
using Api.Entities.Event;
using Api.Extensions;
using Api.Models.Common;
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
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == model.CreatorUsername);
        if (user == null)
            throw new ApplicationException($"User with username {model.CreatorUsername} does not exist.");
        
        var evnt = new Entities.Event.Event()
        {
            CreatorId = user.Id,
            Title = model.Title,
            ImageBase64 = model.ImageBase64,
            Description = model.Description,
            StartDate = model.StartDate.DropSecondsAndNormalize(),
            EndDate = model.EndDate.DropSecondsAndNormalize(),
            UpdatedAt = null,
            CreatedAt = timeProvider.GetUtcNow().UtcDateTime
        };

        if (model.Tags != null)
        {
            // Add tags
            await dbContext.EventTags.AddRangeAsync(model.Tags.Select(x => new EventTag
            {
                TagId = x,
                EventId = evnt.Id
            }));
        }

        await dbContext.AddAsync(evnt);
        await dbContext.SaveChangesAsync();
        return evnt.ToDto();
    }

    public async Task<Models.Event.Event> UpdateEvent(UpdateEvent model)
    {
        var evnt = await dbContext.Events.FirstOrDefaultAsync(x => x.Id == model.EventId);
        if (evnt == null)
            throw new ApplicationException($"Event with id {model.EventId} does not exist.");
        
        evnt.Title = model.Title;
        evnt.Description = model.Description;
        evnt.ImageBase64 = model.ImageBase64;
        evnt.StartDate = model.StartDate.DropSecondsAndNormalize();
        evnt.EndDate = model.EndDate.DropSecondsAndNormalize();
        evnt.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;

        if (model.Tags != null)
        {
            // Remove previously added tags
            await dbContext.EventTags.Where(x => model.Tags.Any(y => x.TagId == y ))
                .ExecuteDeleteAsync();
            await dbContext.SaveChangesAsync();
            
            // Add newly selected tags
            await dbContext.EventTags.AddRangeAsync(model.Tags.Select(x => new EventTag
            {
                TagId = x,
                EventId = evnt.Id
            }));
        }
        
        await dbContext.SaveChangesAsync();
        return evnt.ToDto();
    }

    public async Task<IEnumerable<Models.Event.Event>> GetEventsByUsername(string username)
    {
        var events = await dbContext.Events.Include(x => x.Creator)
            .Where(x => x.Creator.Username == username)
            .Select(x => x.ToDto()).ToListAsync();
        
        return events;
    }

    public async Task<IEnumerable<Models.Event.Event>> SearchEvents(SearchModel model)
    {
        IQueryable<Entities.Event.Event> events = dbContext.Events;
        
        if (!string.IsNullOrEmpty(model.Title)) 
            events = events.Where(x => x.Title.Contains(model.Title));
        
        if (!string.IsNullOrEmpty(model.CreatorName)) 
            events = events.Include(x => x.Creator)
                .Where(x => x.Creator.Username.Contains(model.CreatorName));
        
        if (model.StartDate != null) 
            events = events.Where(x => x.StartDate == model.StartDate);
        
        if (model.EndDate != null) 
            events = events.Where(x => x.EndDate == model.EndDate);
        
        if (model.Tags != null) 
            events = events.Include(x => x.Tags)
                .Where(x => x.Tags.Any(x => model.Tags.Any(tg => tg == x.TagId)));
        
        return await events.Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetCommonTags()
    {
        var tags = await dbContext.Tags.Select(x => x.ToDto())
            .ToListAsync();

        return tags;
    }
}
using Api.Data;
using Api.Entities.Event;
using Api.Extensions;
using Api.Messaging;
using Api.Models.Common;
using Api.Models.Event;
using Api.Services.Notification;
using Api.Services.Redis;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Event;

public class EventService(LiverpoolDbContext dbContext, ILiverpoolRedis redis, INotificationService notificationService,
    TimeProvider timeProvider) : IEventService
{
    public async Task<Models.Event.Event> GetEventById(long id)
    {
        var evnt = await dbContext.Events.Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id);
        
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
        
        await dbContext.AddAsync(evnt);
        await dbContext.SaveChangesAsync();
        
        if (model.Tags != null)
        {
            // Add tags
            await dbContext.EventTags.AddRangeAsync(model.Tags.Select(x => new EventTag
            {
                TagId = x,
                EventId = evnt.Id
            }));
        }
        
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
        
        var attendees = await dbContext.Attendee.Include(x => x.User)
            .Where(x => x.EventId == model.EventId)
            .Select(x => x.User!.Username)
            .ToListAsync();
        
        foreach (var attendee in attendees)
            await notificationService.NotifyEventChanged(attendee, model.EventId);
        
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
        IQueryable<Entities.Event.Event> events = dbContext.Events
            .Where(x => x.CreatorId != model.SearcherId);
        
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

    public async Task<IEnumerable<EventChatPreview>> GetEventChatPreviews(long userId)
    {
        var eventPreviews = await dbContext.Events.Include(x => x.Attendees)
            .Where(x => x.Attendees!.Any(a => a.UserId == userId && a.Status == Entities.Event.AttendeeStatus.Approved) 
                        || x.CreatorId == userId)
            .Select(x => x.ToChatPreview())
            .ToListAsync();
        
        if (eventPreviews.Count == 0)
            return eventPreviews;
        
        var db = redis.GetDatabase();
        foreach (var eventPreview in eventPreviews)
        {
            var message = await db.ListGetByIndexAsync($"Messages:{eventPreview.Id}", -1);
            
            if (!message.HasValue)
                continue;
            
            var messageObject = MessagePackSerializer.Deserialize<ChatMessage>((byte[])message);
        
            eventPreview.LastMessage = messageObject;
        }

        // var eventPreviews = new List<EventChatPreview>()
        // {
        //     new EventChatPreview
        //     {
        //         Id = 4,
        //         Title = "Testing event",
        //         LastMessage = new ChatMessage
        //         {
        //             SentAt = new DateTime(2025, 6, 10),
        //             Message = "Hello World!",
        //             Username = "dude",
        //             MessageId = Guid.NewGuid(),
        //             EventId = 4
        //         }
        //     },
        //     new EventChatPreview
        //     {
        //         Id = 5,
        //         Title = "Image event!",
        //         LastMessage = new ChatMessage
        //         {
        //             SentAt = new DateTime(2025, 12, 31),
        //             Message = "Hello World!",
        //             AttachmentBase64 = "something image",
        //             Username = "dude",
        //             MessageId = Guid.NewGuid(),
        //             EventId = 5
        //         }
        //     }
        // };
        return eventPreviews;
    }
}
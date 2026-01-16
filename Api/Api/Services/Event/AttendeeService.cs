using Api.Data;
using Api.Models.Event;
using Api.Entities.Event;
using Api.Messaging;
using Api.Services.Chat;
using Api.Services.Notification;
using Microsoft.EntityFrameworkCore;
using Attendee = Api.Entities.Event.Attendee;
using AttendeeStatus = Api.Entities.Event.AttendeeStatus;

namespace Api.Services.Event;

public class AttendeeService(LiverpoolDbContext dbContext, INotificationService notificationService,
    IChatService chatService) : IAttendeeService
{
    public async Task<Models.Event.Attendee?> GetAttendee(long userId, long eventId)
    {
        var attendee = await dbContext.Attendee
            .FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);

        return attendee?.ToDto();
    }

    public async Task<Models.Event.Attendee> ApplyToAttend(ParticipationModel model)
    {
        var evnt = await dbContext.Events.Where(x => x.Id == model.EventId)
            .Include(x => x.Creator).FirstOrDefaultAsync();
        
        if (evnt == null)
            throw new ApplicationException($"Event {model.EventId} does not exist.");
        
        var attendee = await dbContext.Attendee
            .FirstOrDefaultAsync(x => x.UserId == model.UserId && x.EventId == model.EventId);

        var creatorUsername = evnt.Creator!.Username;
        if (attendee != null)
        {
            if (attendee.Status is AttendeeStatus.Declined or AttendeeStatus.Removed 
                or AttendeeStatus.Approved or AttendeeStatus.Pending)
                return attendee.ToDto();
            
            attendee.Status = AttendeeStatus.Pending;
            await notificationService.NotifyNewAttendee(creatorUsername, model.EventId);
            await dbContext.SaveChangesAsync();
            return attendee.ToDto();
        }

        attendee = new Attendee()
        {
            UserId = model.UserId,
            EventId = model.EventId,
            Status = AttendeeStatus.Pending,
        };
        
        await notificationService.NotifyNewAttendee(creatorUsername, model.EventId);
        dbContext.Attendee.Add(attendee);
        await dbContext.SaveChangesAsync();
        
        return attendee.ToDto();
    }

    public async Task<List<Models.Event.Attendee>> GetAttendees(GetAttendees model)
    {
        var attendees = dbContext.Attendee
            .Where(x => x.EventId == model.EventId && x.Status == (AttendeeStatus)model.AttendeeStatus);
        
        if (model.GetCreator)
            attendees = attendees.Where(x => x.Status == AttendeeStatus.None);
        
        return await attendees.Include(x => x.User).Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<Models.Event.Attendee> ReviewAttendance(ParticipationReviewModel model)
    {
        var attendee = await dbContext.Attendee.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == model.UserId && x.EventId == model.EventId);
        if (attendee == null)
            throw new ApplicationException($"Attendee {model.UserId} does not exist for event {model.EventId}.");

        var username = attendee.User.Username;
        
        // Unique case. User removal
        if (attendee.Status == AttendeeStatus.Approved & !model.IsAccepted)
        {
            await notificationService.NotifyUserRemoved(username, model.EventId);
            attendee.Status = AttendeeStatus.Removed;
        }
        else if (model.IsAccepted)
        {
            await chatService.SendMessage(new ChatMessage
            {
                EventId = model.EventId,
                Username = username,
                Message = $"User {username} has joined the chat.",
                Type = ChatMessageType.System
            });
            
            await notificationService.NotifyUserAccepted(username, model.EventId);
            attendee.Status = AttendeeStatus.Approved;
        }
        else
        {
            await notificationService.NotifyUserRejected(username, model.EventId);
            attendee.Status = AttendeeStatus.Declined;
        }
        
        await dbContext.SaveChangesAsync();
        return attendee.ToDto();
    }

    public async Task<Models.Event.Attendee> UnattendEvent(UnattendModel model)
    {
        var attendee = await dbContext.Attendee.Include(x => x.User)
            .Where(x => x.EventId == model.EventId && x.UserId == model.UserId).FirstOrDefaultAsync();
        
        if (attendee == null)
            throw new ApplicationException($"Attendee {model.UserId} does not exist.");
        
        attendee.Status = AttendeeStatus.Left;

        var creatorUsername = await dbContext.Events.Include(x => x.Creator)
            .Select(x => x.Creator.Username).FirstOrDefaultAsync();
        
        await chatService.SendMessage(new ChatMessage
        {
            EventId = model.EventId,
            Username = attendee.User.Username,
            Message = $"User {attendee.User.Username} has left the chat.",
            Type = ChatMessageType.System
        });
        
        await notificationService.NotifyUserLeft(creatorUsername, model.EventId);
        await dbContext.SaveChangesAsync();
        
        return attendee.ToDto();
    }
}
using System.Text.Json;
using Api.Messaging;
using Api.Messaging.SignalR;
using Api.Services.Redis;
using MessagePack;
using Microsoft.AspNetCore.SignalR;

namespace Api.Services.Notification;

public class NotificationService(IHubContext<NotificationHub> hubContext, ILiverpoolRedis redis) : INotificationService
{
    public async Task<Messaging.Notification> NotifyUserAccepted(string username, long eventId) 
        => await Notify(NotificationType.EventUserAccepted, username, eventId);

    public async Task<Messaging.Notification> NotifyUserRejected(string username, long eventId)
        => await Notify(NotificationType.EventUserDenied, username, eventId);

    public async Task<Messaging.Notification> NotifyUserRemoved(string username, long eventId)
        => await Notify(NotificationType.EventUserRemoved, username, eventId);

    public async Task<Messaging.Notification> NotifyEventCancelled(string username, long eventId)
        => await Notify(NotificationType.EventCancelled, username, eventId);

    public async Task<Messaging.Notification> NotifyEventUpcoming(string username, long eventId)
        => await Notify(NotificationType.EventUpcoming, username, eventId);

    public async Task<Messaging.Notification> NotifyEventChanged(string username, long eventId)
        => await Notify(NotificationType.EventChanged, username, eventId);
    
    public async Task<Messaging.Notification> NotifyNewAttendee(string username, long eventId)
        => await Notify(NotificationType.NewAttendee, username, eventId);
    
    public async Task<Messaging.Notification> NotifyNewMessage(string username, long eventId)
        => await Notify(NotificationType.NewMessage, username, eventId);
    
    public async Task<Messaging.Notification> Notify(NotificationType type, string username, long? eventId = null)
    {
        var db = redis.GetDatabase();
        var message = new Messaging.Notification
        {
            NotificationType = type,
            EventId = eventId
        };
        
        var connectionId = await db.StringGetAsync($"{NotificationHub.REDIS_PREFIX}:{username}");

        // Store message in redis list for later
        if (!connectionId.HasValue)
        {
            var packedMessage = MessagePackSerializer.Serialize(message);
            await db.SetAddAsync("PendingNotifications", username);
            await db.ListLeftPushAsync($"Notifications:{username}", packedMessage);
            
            return message;
        }
        
        var msgJson = JsonSerializer.Serialize(message);
        await hubContext.Clients.Group(username).SendAsync("ReceiveNotification", msgJson);
        return message;
    }

    public async Task<IEnumerable<Messaging.Notification>> GetNotifications(string username)
    { 
        var db = redis.GetDatabase();
        var notifications = await db.ListRangeAsync($"Notifications:{username}");
        
        return notifications.Select(x => MessagePackSerializer
            .Deserialize<Messaging.Notification>((byte[])x)).ToList();
    }
}
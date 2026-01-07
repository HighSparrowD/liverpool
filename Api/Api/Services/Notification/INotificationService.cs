using Api.Messaging;

namespace Api.Services.Notification;

public interface INotificationService
{
    Task<Messaging.Notification> NotifyUserAccepted(string username, long eventId);
    
    Task<Messaging.Notification> NotifyUserRejected(string username, long eventId);
    
    Task<Messaging.Notification> NotifyUserRemoved(string username, long eventId);
    
    Task<Messaging.Notification> NotifyNewAttendee(string username, long eventId);
    
    Task<Messaging.Notification> NotifyEventCancelled(string username, long eventId);
    
    Task<Messaging.Notification> NotifyEventUpcoming(string username, long eventId);
    
    Task<Messaging.Notification> NotifyEventChanged(string username, long eventId);

    Task<Messaging.Notification> NotifyNewMessage(string username, long eventId);
    
    Task<Messaging.Notification> Notify(NotificationType type, string username, long? eventId = null);
    
    Task<IEnumerable<Messaging.Notification>> GetNotifications(string username);
}
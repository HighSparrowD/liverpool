using System.Text.Json;
using Api.Data;
using Api.Messaging;
using Api.Messaging.SignalR;
using Api.Services.Notification;
using Api.Services.Redis;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Chat;

public class ChatService(LiverpoolDbContext dbContext, IHubContext<MessagingHub> hubContext, 
    ILiverpoolRedis redis, TimeProvider timeProvider, INotificationService notificationService) : IChatService
{
    public async Task<ChatMessage> SendMessage(ChatMessage message)
    {
        var db = redis.GetDatabase();

        var attendees = await dbContext.Attendee.Include(x => x.User)
            .Where(x => x.EventId == message.EventId && x.User!.Username != message.Username)
            .Select(x => x.User!.Username).ToListAsync();

        foreach (var attendee in attendees) 
            await notificationService.NotifyNewMessage(attendee, message.EventId);
        
        message.SentAt = timeProvider.GetUtcNow().UtcDateTime;
        var packedMessage = MessagePackSerializer.Serialize(message);
        var jsonMessage = JsonSerializer.Serialize(message);
        
        await hubContext.Clients.Group(message.EventId.ToString()).SendAsync("ReceiveMessage", jsonMessage);
        await db.ListRightPushAsync($"Messages:{message.EventId}", packedMessage);
        
        return message;
    }

    public async Task<IEnumerable<ChatMessage>> GetAllMessages(long eventId)
    {
        var db = redis.GetDatabase();
        var messages = await db.ListRangeAsync($"Messages:{eventId}");

        var messagesList = new List<ChatMessage>();
        foreach (var message in messages)
        {
            if (!message.HasValue)
                continue;
            
            var messageObject = MessagePackSerializer.Deserialize<ChatMessage>((byte[])message);
            messagesList.Add(messageObject);
        }
        
        return messagesList;
    }

    public Task<ChatMessage> DeleteMessage(long eventId, long messageId)
    {
        throw new NotImplementedException();
    }
}
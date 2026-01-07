using Api.Services.Chat;
using Api.Services.Redis;
using Microsoft.AspNetCore.SignalR;

namespace Api.Messaging.SignalR;

public class MessagingHub(ILiverpoolRedis redis, IChatService chatService) : Hub
{
    public async Task Subscribe(long eventId, string username)
    {
        var connectionId = Context.ConnectionId;
        
        await Groups.AddToGroupAsync(connectionId, eventId.ToString());
    }

    public async Task SendMessage(ChatMessage message)
    {
        await chatService.SendMessage(message);
    }
}
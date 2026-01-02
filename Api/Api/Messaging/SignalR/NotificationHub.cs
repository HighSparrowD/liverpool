using Api.Services.Redis;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace Api.Messaging.SignalR;

public class NotificationHub(ILiverpoolRedis redis) : Hub
{
    public async Task Subscribe(string username)
    {
        var connectionId = Context.ConnectionId;
        var db = redis.GetDatabase();
        await db.StringSetAsync(connectionId, username);
        await db.StringSetAsync(username, connectionId);
        await Groups.AddToGroupAsync(connectionId, username);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var db = redis.GetDatabase();
        var connId = Context.ConnectionId;

        var username = await db.StringGetAsync(connId);
        
        await db.StringDeleteAsync(connId, ValueCondition.Exists);
        await db.StringDeleteAsync(username.ToString(), ValueCondition.Exists);
        
        await base.OnDisconnectedAsync(exception);
    }
}
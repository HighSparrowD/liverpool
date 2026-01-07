using Api.Services.Redis;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace Api.Messaging.SignalR;

public class NotificationHub(ILiverpoolRedis redis) : Hub
{
    public const string REDIS_PREFIX = "NotificationHub";
    
    public async Task Subscribe(string username)
    {
        var connectionId = Context.ConnectionId;
        var db = redis.GetDatabase();
        
        await db.StringSetAsync($"{REDIS_PREFIX}:{connectionId}", username);
        await db.StringSetAsync($"{REDIS_PREFIX}:{username}", connectionId);
        
        await Groups.AddToGroupAsync(connectionId, username);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var db = redis.GetDatabase();
        var connId = Context.ConnectionId;
            
        var redisKey =  $"{REDIS_PREFIX}:{connId}";
        var username = await db.StringGetAsync(redisKey);
        
        await db.StringDeleteAsync(redisKey, ValueCondition.Exists);
        await db.StringDeleteAsync($"{REDIS_PREFIX}:{username.ToString()}", ValueCondition.Exists);
        
        await base.OnDisconnectedAsync(exception);
    }
}
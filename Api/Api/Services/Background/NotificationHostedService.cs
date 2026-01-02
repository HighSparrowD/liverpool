using System.Text;
using Api.Services.Notification;
using Api.Services.Redis;
using MessagePack;
using StackExchange.Redis;

namespace Api.Services.Background;

public class NotificationHostedService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private IDatabase _db;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var redis = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILiverpoolRedis>();
        _db = redis.GetDatabase();
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var leftInQueue = true;
            var pendingUsers = new List<string>();
            while (leftInQueue)
            {
                var user = await _db.SetPopAsync("PendingMessages");
                if (!user.HasValue)
                {
                    leftInQueue = false;
                    continue;
                }

                var username = user.ToString();
                
                var connectionId = await _db.StringGetAsync(username);
                if (!connectionId.HasValue)
                {
                    // return pending message to queue
                    pendingUsers.Add(username);
                    continue;
                }
                
                var msgsLeft = true;
                var scope = scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                while (msgsLeft)
                {
                    var message = await _db.ListLeftPopAsync($"Messages:{username}");
                    if (!message.HasValue)
                    {
                        msgsLeft = false;
                        continue;
                    }
                    
                    var msg = MessagePackSerializer.Deserialize<Messaging.Notification>((byte[])message);
                    await notificationService.Notify(msg.NotificationType, username, msg.EventId);
                }
            }

            foreach (var user in pendingUsers)
            {
                await _db.SetAddAsync("PendingMessages", user);
            }
            await Task.Delay(5000, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
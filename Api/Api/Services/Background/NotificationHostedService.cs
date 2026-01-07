using System.Text;
using Api.Messaging.SignalR;
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

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
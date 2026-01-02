using MessagePack;

namespace Api.Messaging;

[MessagePackObject]
public record Notification
{
    [Key(0)] public NotificationType NotificationType { get; set; }

    [Key(1)] public long? EventId { get; set; }
}
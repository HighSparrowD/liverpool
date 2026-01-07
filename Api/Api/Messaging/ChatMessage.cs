using System.ComponentModel.DataAnnotations;
using MessagePack;

namespace Api.Messaging;

[MessagePackObject]
public record ChatMessage
{
    [MessagePack.Key(0)] public Guid MessageId { get; set; } = Guid.NewGuid();
    
    [MessagePack.Key(1)] public required long EventId { get; set; }
    
    [MessagePack.Key(2)] public required string Username { get; set; }
    
    [MessagePack.Key(3)] public string Message { get; set; }
    
    [MessagePack.Key(4)] public DateTime SentAt { get; set; }
    
    [MessagePack.Key(5)] public long? RepliedTo { get; set; }
    
    [MessagePack.Key(6), Base64String] public string? AttachmentBase64 { get; set; }
}
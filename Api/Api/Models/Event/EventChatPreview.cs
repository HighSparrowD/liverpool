using Api.Messaging;

namespace Api.Models.Event;

public record EventChatPreview()
{
    public required long Id { get; set; }
    
    public required string Title { get; set; }

    public ChatMessage? LastMessage { get; set; }
}
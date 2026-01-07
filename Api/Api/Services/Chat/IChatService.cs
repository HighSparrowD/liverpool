using Api.Messaging;

namespace Api.Services.Chat;

public interface IChatService
{
    Task<ChatMessage> SendMessage(ChatMessage message);
    
    Task<IEnumerable<ChatMessage>> GetAllMessages(long eventId);
    
    Task<ChatMessage> DeleteMessage(long eventId, long messageId);
}
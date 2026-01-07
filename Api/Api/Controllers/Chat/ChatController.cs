using Api.Messaging;
using Api.Models.Event;
using Api.Services.Chat;
using Api.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Chat;

[Route("/api/chat")]
public class ChatController(IEventService eventService, IChatService chatService) : Controller
{
    [HttpGet("events")]
    public async Task<ActionResult<List<EventChatPreview>>> GetAllChats([FromQuery] long userId)
    {
        var chatPreviews = await eventService.GetEventChatPreviews(userId);
        
        return Ok(chatPreviews);
    }
    
    [HttpGet("event")]
    public async Task<ActionResult<List<ChatMessage>>> Get([FromQuery] long eventId)
    {
        var eventChat = await chatService.GetAllMessages(eventId);
        return Ok(eventChat);
    }
}
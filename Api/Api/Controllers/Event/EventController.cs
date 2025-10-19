using Api.Models.Event;
using Api.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Event;

[Route("/api/event")]
public class EventController(IEventService eventService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<Models.Event.Event>> GetEvent([FromQuery] long id)
    {
        var evnt = await eventService.GetEventById(id);
        
        return Ok(evnt);
    }
    
    // [HttpGet]
    // public async Task<ActionResult<List<Models.Event.Event>>> GetEvents([FromQuery] long creatorId)
    // {
    //     
    // }
    
    [HttpPost]
    public async Task<ActionResult<Models.Event.Event>> CreateEvent([FromBody] CreateEvent model)
    {
        var evnt = await eventService.CreateEvent(model);
        return Ok(evnt);
    }
}
using Api.Models.Event;
using Api.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Event;

[Route("/api/event")]
public class EventController(IEventService eventService) : Controller
{
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Models.Event.Event>>> GetUserEvents([FromQuery] string username)
    {
        var events = await eventService.GetEventsByUsername(username);
        return Ok(events);
    }
    
    [HttpGet]
    public async Task<ActionResult<Models.Event.Event>> GetEvent([FromQuery] long id)
    {
        var evnt = await eventService.GetEventById(id);
        
        return Ok(evnt);
    }
    
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<Models.Event.Event>>> GetEvents([FromBody] SearchModel model)
    {
        var evnts = await eventService.SearchEvents(model);

        return Ok(evnts);
    }
    
    [HttpPost]
    public async Task<ActionResult<Models.Event.Event>> CreateEvent([FromBody] CreateEvent model)
    {
        var evnt = await eventService.CreateEvent(model);
        return Ok(evnt);
    }
    
    [HttpPut]
    public async Task<ActionResult<Models.Event.Event>> UpdateEvent([FromBody] UpdateEvent model)
    {
        var evnt = await eventService.UpdateEvent(model);
        return Ok(evnt);
    }
    
    [HttpGet("common/tags")]
    public async Task<ActionResult<Models.Common.Tag>> GetCommonTags()
    {
        var tags = await eventService.GetCommonTags();
        return Ok(tags);
    }
}
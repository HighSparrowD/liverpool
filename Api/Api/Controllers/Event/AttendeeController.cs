using Api.Models.Event;
using Api.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Event;

[Route("/api/attendee")]
public class AttendeeController(IAttendeeService attendeeService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<Models.Event.Attendee?>> GetAttendee([FromQuery] long userId, [FromQuery] long eventId)
    {
        var attendee = await attendeeService.GetAttendee(userId, eventId);
        return Ok(attendee);
    }
    
    [HttpPost("attend")]
    public async Task<ActionResult<Models.Event.Attendee?>> ApplyToAttend([FromQuery] ParticipationModel model)
    {
        var attendee = await attendeeService.ApplyToAttend(model);
        return Ok(attendee);
    }
}
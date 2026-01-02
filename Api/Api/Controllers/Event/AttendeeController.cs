using Api.Models.Event;
using Api.Services.Event;
using Api.Services.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Event;

[Route("/api/attendee")]
public class AttendeeController(IAttendeeService attendeeService, INotificationService notificationService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<Attendee?>> GetAttendee([FromQuery] long userId, [FromQuery] long eventId)
    {
        var attendee = await attendeeService.GetAttendee(userId, eventId);
        return Ok(attendee);
    }
    
    [HttpPost("list")]
    public async Task<ActionResult<List<Attendee>>> GetAttendees([FromBody] GetAttendees model)
    {
        var attendee = await attendeeService.GetAttendees(model);
        return Ok(attendee);
    }
    
    [HttpPost("attend")]
    public async Task<ActionResult<Attendee?>> ApplyToAttend([FromQuery] ParticipationModel model)
    {
        var attendee = await attendeeService.ApplyToAttend(model);
        return Ok(attendee);
    }
    
    [HttpPost("review")]
    public async Task<ActionResult<Attendee?>> Review([FromQuery] ParticipationReviewModel model)
    {
        var attendee = await attendeeService.ReviewAttendance(model);
        return Ok(attendee);
    }
}
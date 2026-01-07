using Api.Messaging;
using Api.Services.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User;

[Route("/api/notification")]
public class NotificationController(INotificationService notificationService) : Controller
{
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Notification>>> GetAllAsync(string username)
    {
        var notifications = await notificationService.GetNotifications(username);
        return Ok(notifications);
    }
}
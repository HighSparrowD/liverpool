using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User;

[Route("/api/user")]
public class UserController : Controller
{
    [HttpGet]
    public ActionResult<IEnumerable<Models.User.User>> GetUsers()
    {
        return Ok(new List<Models.User.User>());
    }
}
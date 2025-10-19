using Api.Models.User;
using Api.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User;

[Route("/api/user")]
public class UserController(IRegistrationService registrationService) : Controller
{
    [HttpGet]
    public ActionResult<IEnumerable<Models.User.User>> GetUsers()
    {
        return Ok(new List<Models.User.User>());
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<Models.User.User>> CreateUser([FromBody] CreateUser model)
    {
        var user = await registrationService.CreateUser(model);
        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<Models.User.User>> LoginUser([FromBody] LoginUser model)
    {
        var user = await registrationService.LoginUser(model);
        return Ok(user);
    }
}
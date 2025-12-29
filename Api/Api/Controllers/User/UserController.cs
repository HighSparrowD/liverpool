using Api.Models.User;
using Api.Services;
using Api.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User;

[Route("/api/user")]
public class UserController(IRegistrationService registrationService, IUserService userService) : Controller
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
    
    [HttpPut]
    public async Task<ActionResult<Models.User.User>> UpdateUser([FromBody] UpdateUser model)
    {
        var user = await registrationService.UpdateUser(model);
        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<Models.User.User>> LoginUser([FromBody] LoginUser model)
    {
        
        var user = await registrationService.LoginUser(model);
        return Ok(user);
    }
    
    [HttpGet("profile")]
    public async Task<ActionResult<Models.User.User>> GetUser([FromQuery] string username)
    {
        var user = await userService.GetUser(username);
        return Ok(user);
    }
}
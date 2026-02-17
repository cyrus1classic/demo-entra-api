using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers;

[ApiController]
[Authorize]
[Route("secure")]
public class SecureController : ControllerBase
{
    [HttpGet("read")]
    [Authorize(Policy = "Scopes:access_as_user_OR_Roles:Demo.Read")]
    public IActionResult Read() => Ok(new { message = "You are authorized for read access." });

    [HttpPost("admin")]
    [Authorize(Policy = "Roles:Demo.Admin")]
    public IActionResult Admin() => Ok(new { message = "You are authorized for admin access." });
}

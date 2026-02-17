using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers;

[ApiController]
[Authorize]
[Route("me")]
public class MeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new
        {
            name = User.Identity?.Name,
            subject = User.FindFirstValue("sub"),
            tenantId = User.FindFirstValue("tid"),
            claims
        });
    }
}

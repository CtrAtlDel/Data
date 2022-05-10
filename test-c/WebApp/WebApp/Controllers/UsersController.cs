using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("I am a public ");
    }
    
    
}
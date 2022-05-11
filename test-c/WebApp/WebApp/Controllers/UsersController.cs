using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("admin")]
    public IActionResult ifAdmin()
    {
        var user = GetUserLogin();
        return Ok($"I am a admin {user.Login} ");
    }
    
    [HttpGet("user")]
    public IActionResult ifUser()
    {
        var user = GetUserLogin();
        return Ok($"I am a user {user.Login} ");
    }


    private User GetUserLogin()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            var userClaim = identity.Claims;
            return new User
            {
                Login = userClaim.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value
            };
        }

        return null;
    }
    
}
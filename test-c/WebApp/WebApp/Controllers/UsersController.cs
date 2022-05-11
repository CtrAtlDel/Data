using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    
    //Create
    [HttpPost] // 1)
    public IActionResult CreateUser(string Login, string Password, UserCreate userCreator) // Admin
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(userCreator);
    }
    
    //Update - 1
    [HttpPut("UpdateAll")] // 2)
    public IActionResult UpdateAll(string Login, string Password)
    {
        return Ok();
    }

    [HttpPut("UpdatePassword")] // 3)
    public IActionResult UpdatePassword(string Login, string Password)
    {
        return Ok();
    }
    
    [HttpPut("UpdateLogin")] // 4)
    public IActionResult UpdateLogin(string Login, string Password)
    {
        return Ok();
    }

    //Read
    [HttpGet] // 5)
    public IEnumerable<User> Get(string Login, string Password) // For administrator
    {
        if (ifAdmin(Login))
        {
            return UsersReposiroty.db;
        }

        return null;
    }

    [HttpGet("GetLogin")]  // 6)
    public User GetUser(string Login, string Password, string UserLogin) // For administrator
    {
        return null;
    }

    [HttpGet("GetUserInfo")] // 7)
    public User GetUserInfo(string Login, string Password, string UserLogin, string UserPassword)
    {
        return null;
    }

    [HttpGet("GetUserByAge")] // 8)

    public IEnumerable<User> GetAge(string Login, string Password, DateTime birthday)
    {
        return null;
    }
    
    //Delete 

    [HttpDelete("DeleteSoft")] // 9.1)
    public IActionResult DeleteSoft(string Login, string Password, string UserLogin)
    {
        return Ok();
    }

    [HttpDelete("DeleteHard")] // 9.2)
    public IActionResult DeleteHard(string Login, string Password, string UserLogin)
    {
        return Ok();
    }

    //Update - 2
    [HttpPut("RestoreUser")] // 10) Параметры пользователя не указаны? => будем по логину
    public IActionResult RestoreUser(string Login, string Password,  string UserLogin)
    {
        return Ok();
    }
    
    private bool ifAdmin(string Login)
    {
        var user = UsersReposiroty.db.SingleOrDefault(u => u.Login == Login);
        return user.Admin;
    }

}
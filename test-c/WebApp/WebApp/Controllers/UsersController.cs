using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    
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

    [HttpGet("Login")]  // 6)
    public User GetUser(string Login, string Password, string UserLogin) // For administrator
    {
        return null;
    }

    [HttpGet("UserInfo")] // 7)
    public User GetUserInfo(string Login, string Password, string UserLogin, string UserPassword)
    {
        return null;
    }

    [HttpGet("Age")]

    public IEnumerable<User> GetAge(string Login, string Password, DateTime birthday)
    {
        return null;
    }


    private bool ifAdmin(string Login)
    {
        var user = UsersReposiroty.db.SingleOrDefault(u => u.Login == Login);
        return user.Admin;
    }

}
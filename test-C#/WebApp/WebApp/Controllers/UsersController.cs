using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    //Create
    [HttpPost] // 1)
    public IActionResult CreateUser(string login, string password, UserCreate userCreator) // Admin
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = userCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (!userSession.Admin) return BadRequest("Access denied");

        if (!userLoginCheck(userCreator.Login))
            return BadRequest("User with this nickname is exist");

        var user = new User();
        user.Login = userCreator.Login;
        user.Password = userCreator.Password;
        user.Name = userCreator.Name;
        user.Gender = userCreator.Gender;
        user.Birthday = userCreator.Birthday;
        user.Admin = userCreator.Admin;
        user.CreatedBy = login;
        user.CreatedOn = DateTime.Now;
        UsersReposiroty.db.Add(user);

        return Ok(userCreator);
    }

    //Update - 1  
    [HttpPut("UpdateAll")] // 2) 
    public IActionResult UpdateAll(string login, string password, string userLogin, string userPassword,
        UserUpdate userUpdate)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = userCheck(login, password);

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        var userData = userCheck(userLogin, userPassword);

        if (userSession == null || userData == null) return BadRequest("Login or password is incorrect");
        if (!userSession.Admin)
        {
            if (login != userLogin && password != userPassword)
            {
                return BadRequest("Access denied, you cannot change another user's data");
            }
        }

        foreach (var it in UsersReposiroty.db.Where(it => it.Login == userLogin))
        {
            it.Name = userUpdate.Name;
            it.Gender = userUpdate.Gender;
            it.Birthday = userUpdate.Birthday;
        }

        return Ok();
    }

    [HttpPut("UpdatePassword")] // 3)
    public IActionResult UpdatePassword(string login, string password, string userLogin, string userPassword,
        string newPassword)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = userCheck(login, password);

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        var userData = userCheck(userLogin, userPassword);

        if (userSession == null || userData == null) return BadRequest("Login or password is incorrect");
        if (!userSession.Admin)
        {
            if (login != userLogin && password != userPassword)
            {
                return BadRequest("Access denied, you cannot change another user's data");
            }
        }

        foreach (var it in UsersReposiroty.db.Where(it => it.Login == userLogin))
        {
            it.Password = newPassword;
        }


        return Ok("Success");
    }

    [HttpPut("UpdateLogin")] // 4)
    public IActionResult UpdateLogin(string login, string password, string userLogin, string userPassword,
        string newLogin)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = userCheck(login, password);

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        var userData = userCheck(userLogin, userPassword);

        if (userSession == null || userData == null) return BadRequest("Login or password is incorrect");
        if (!userSession.Admin)
        {
            if (login != userLogin && password != userPassword)
            {
                return BadRequest("Access denied, you cannot change another user's data");
            }
        }

        if (UsersReposiroty.db.Any(it => it.Login == newLogin))
        {
            return BadRequest("User with this login already exists");
        }

        UsersReposiroty.db.FindAll(user => user.Login == userLogin).ForEach(o => o.Login = newLogin);

        return Ok("Success");
    }

    //Read 
    [HttpGet] // 5)
    public IActionResult Get(string login, string password)
    {
        if (!ifAdmin(login, password)) return BadRequest("Access denied");

        return Ok(UsersReposiroty.db.OrderBy(o => o.CreatedOn.Date).ToArray());
    }

    [HttpGet("GetLogin")] // 6)
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
    public IActionResult DeleteSoft(string login, string password, string userLogin)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");
        var userSession = userCheck(login, password);
        if (userSession == null) return BadRequest("Login or password is incorrect");
        if (!userSession.Admin) return BadRequest("Access denied");

        return Ok();
    }

    [HttpDelete("DeleteHard")] // 9.2)
    public IActionResult DeleteHard(string Login, string Password, string UserLogin)
    {
        return Ok();
    }

    //Update - 2
    [HttpPut("RestoreUser")] // 10) Параметры пользователя не указаны? => будем по логину
    public IActionResult RestoreUser(string Login, string Password, string UserLogin)
    {
        return Ok();
    }

    private User userCheck(string login, string password)
    {
        return UsersReposiroty.db.FirstOrDefault(it => it.Login == login && it.Password == password);
    }

    private bool userLoginCheck(string login)
    {
        return UsersReposiroty.db.All(it => it.Login != login);
    }

    private static bool ifAdmin(string login, string password)
    {
        var user = UsersReposiroty.db.SingleOrDefault(u => u.Login == login && u.Password == password);
        return user.Admin;
    }
}
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

        var user = new User
        {
            Login = userCreator.Login,
            Password = userCreator.Password,
            Name = userCreator.Name,
            Gender = userCreator.Gender,
            Birthday = userCreator.Birthday,
            Admin = userCreator.Admin,
            CreatedBy = login,
            CreatedOn = DateTime.Now
        };
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
            return BadRequest("User with this login already exists");

        UsersReposiroty.db.FindAll(user => user.Login == userLogin).ForEach(o => o.Login = newLogin);

        return Ok("Success");
    }

    //Read 
    [HttpGet] // 5) //todo add sorting
    public IActionResult Get(string login, string password)
    {
        if (!ifAdmin(login, password)) return BadRequest("Access denied");

        return Ok(UsersReposiroty.db.OrderBy(o => o.CreatedOn.Date).ToArray());
    }

    [HttpGet("GetLogin")] // 6)
    public IActionResult GetUser(string login, string password, string userLogin)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = userCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (!userSession.Admin) return BadRequest("Access denied");

        var user = UsersReposiroty.db.Find(user => user.Login == userLogin);

        return Ok(new UserGet
        {
            Name = user.Name,
            Gender = user.Gender,
            Birthday = user.Birthday,
            Active = user.RevorkedOn == DateTime.MinValue
        });
    }

    //todo change revorkedOn in empty or null 
    [HttpGet("GetUserInfo")] // 7)
    public IActionResult GetUserInfo(string login, string password)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = userCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (userSession.RevorkedOn == null) return BadRequest("This user was deleted");

        return Ok(new UserGet
        {
            Active = true,
            Name = userSession.Name,
            Gender = userSession.Gender,
            Birthday = userSession.Birthday
        });
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
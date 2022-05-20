using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    //Create
    [HttpPost] // 1)
    public IActionResult CreateUser(string login, string password, UserCreate userCreator)
    {
        if (!ModelState.IsValid) return BadRequest("Is not valid form");

        var userSession = UserCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (!userSession.Admin) return BadRequest("Access denied");

        if (!(IfRegexCheck(userCreator.Login) && IfRegexCheck(userCreator.Password))) //check if statement
            return BadRequest("Incorrect login or password");

        if (!IfRegexCheck(userCreator.Name))
            return BadRequest("Incorrect user name");

        if (!UserLoginCheck(userCreator.Login))
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
            CreatedOn = DateTime.Now,
            RevorkedOn = DateTime.MinValue,
            RevorkedBy = ""
        };
        UsersReposiroty.db.Add(user);

        return Ok(userCreator);
    }

    //Update - 1  
    [HttpPut("UpdateAll")] // 2) 
    public IActionResult UpdateAll(string login, string password, string userLogin, string userPassword,
        UserUpdate userUpdate)
    {
        var userSession = UserCheck(login, password);

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        var userData = UserCheck(userLogin, userPassword);

        if (!IfRegexCheck(userUpdate.Name))
            return BadRequest("Incorrect user name");

        if (userSession == null || userData == null) return BadRequest("Login or password is incorrect");
        if (!userSession.Admin)
        {
            if (login != userLogin && password != userPassword)
                return BadRequest("Access denied, you cannot change another user's data");
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
        var userSession = UserCheck(login, password);

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        var userData = UserCheck(userLogin, userPassword);

        if (userSession == null || userData == null) return BadRequest("Login or password is incorrect");

        if (!IfRegexCheck(userPassword))
            return BadRequest("Incorrect new password");
        
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
        var userSession = UserCheck(login, password);

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        var userData = UserCheck(userLogin, userPassword);

        if (userSession == null || userData == null) return BadRequest("Login or password is incorrect");

        if (!IfRegexCheck(newLogin))
            return BadRequest("New login is incorrect");

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
    [HttpGet] // 5)
    public IActionResult Get(string login, string password)
    {
        if (!IfAdmin(login, password)) return BadRequest("Access denied");

        return Ok(UsersReposiroty.db.FindAll(user => user.RevorkedOn == DateTime.MinValue)
            .OrderBy(o => o.CreatedOn.Date)
            .ToArray());
    }

    [HttpGet("GetLogin")] // 6)
    public IActionResult GetUser(string login, string password, string userLogin)
    {
        var userSession = UserCheck(login, password);

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

    [HttpGet("GetUserInfo")] // 7)
    public IActionResult GetUserInfo(string login, string password)
    {
        var userSession = UserCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (userSession.RevorkedOn != DateTime.MinValue) return BadRequest("This user was deleted");

        return Ok(new UserGet
        {
            Active = true,
            Name = userSession.Name,
            Gender = userSession.Gender,
            Birthday = userSession.Birthday
        });
    }

    [HttpGet("GetUserByAge")] // 8) 
    public IEnumerable<User> GetAge(string login, string password, int age)
    {
        return UsersReposiroty.db.FindAll(user => user.Birthday.Value.Year > age);
    }

    //Delete 
    [HttpDelete("DeleteSoft")] // 9.1)
    public IActionResult DeleteSoft(string login, string password, string userLogin)
    {
        var userSession = UserCheck(login, password);
        if (userSession == null) return BadRequest("Login or password is incorrect");
        if (!userSession.Admin) return BadRequest("Access denied");

        var user = GetUser(userLogin);
        if (user == null) return BadRequest("Cannot find this user login");

        user.RevorkedOn = DateTime.Now;
        user.RevorkedBy = login;

        return Ok("Success soft delete");
    }

    [HttpDelete("DeleteHard")] // 9.2)
    public IActionResult DeleteHard(string login, string password, string userLogin)
    {
        var userSession = UserCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (!userSession.Admin) return BadRequest("Access denied");

        var user = GetUser(userLogin);

        if (user == null) return BadRequest("Cannot find this user login");

        if (user.Admin) return BadRequest("Cannot delete administrator"); //чтобы админ не мог удалить сам себя

        UsersReposiroty.db.Remove(UsersReposiroty.db.SingleOrDefault(user => user.Login == userLogin));

        return Ok("Success hard delete");
    }

    //Update - 2
    [HttpPut("RestoreUser")] // 10) Параметры пользователя не указаны? => будем по логину
    public IActionResult RestoreUser(string login, string password, string userLogin)
    {
        var userSession = UserCheck(login, password);

        if (userSession == null) return BadRequest("Login or password is incorrect");

        if (!userSession.Admin) return BadRequest("Access denied");

        foreach (var it in UsersReposiroty.db.Where(it => it.Login == userLogin))
        {
            it.RevorkedOn = DateTime.MinValue;
            it.RevorkedBy = "";
        }

        return Ok();
    }

    private static bool IfRegexCheck(string inputStr)
    {
        const string regexForLogin = "[A-Za-z0-9]";
        var match = Regex.Match(inputStr, regexForLogin, RegexOptions.IgnoreCase);
        return match.Success;
    }

    private static User UserCheck(string login, string password)
    {
        return UsersReposiroty.db.FirstOrDefault(it => it.Login == login && it.Password == password);
    }

    private static bool UserLoginCheck(string login)
    {
        return UsersReposiroty.db.All(it => it.Login != login);
    }

    private static User GetUser(string login)
    {
        return UsersReposiroty.db.FirstOrDefault(it => it.Login == login);
    }

    private static bool IfAdmin(string login, string password)
    {
        var user = UsersReposiroty.db.SingleOrDefault(u => u.Login == login && u.Password == password);
        return user.Admin;
    }
}
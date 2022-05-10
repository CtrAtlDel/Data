using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    //Database 
    private static List<User> db = new List<User>(new[]
    {
        new User()
        {
            Guid = Guid.NewGuid(), Login = "admin", Password = "admin", Name = "admin", Gender = 1, Birthday = null,
            Admin = true,
            CreatedOn = DateTime.Today, CreatedBy = "admin", ModifiedOn = DateTime.Today, ModifiedBy = "admin",
            RevorkedOn = DateTime.Today, RevorkedBy = "admin"
        }
    });

    [HttpGet]
    public IEnumerable<User> Get() => db;

    public IActionResult Get(Guid id)
    {
        var user = db.SingleOrDefault(user => user.Guid == id);

        if (user == null) return NotFound(); //ERROR 404

        return Ok(user);
    }

    //Create


    //Read

    //Update

    //Delete
}
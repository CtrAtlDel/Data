using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

[Route("/api/[controller]")]
public class UsersController : Controller
{
    //Our database (imitation)
    private static List<User> _users = new List<User>(new[]
    {
        new User()
        {
            Guid = Guid.NewGuid() ,Login = "admin", Password = "admin", Name = "admin", Gender = 1, Birthday = null, Admin = true,
            CreatedOn = DateTime.Today, CreatedBy = "admin", ModifiedOn = DateTime.Today, ModifiedBy = "admin",
            RevorkedOn = DateTime.Today, RevorkedBy = "admin"
        }
    });
    
    //Create
    
    
    
    //Read
    
    //Update
    
    //Delete
    
}
using Microsoft.AspNetCore.Identity;
using WebApp.Model;

namespace WebApp.Services;

public interface IUserService
{
    
    //Create
    public User Create(User user);

    
    public User Get(Login user);
}
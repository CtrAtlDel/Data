using WebApp.Model;

namespace WebApp.Services;

public class UserService : IUserService
{
    public User Get(Login user)
    {
        var userFind = UsersReposiroty.db.FirstOrDefault(o =>
            o.Login.Equals(user.UserLogin, StringComparison.OrdinalIgnoreCase) && user.Password == o.Password);
        return userFind;
    }

    public User Create(User user)
    {
        user.Guid = Guid.NewGuid(); //просто добавление
        UsersReposiroty.db.Add(user);
        return user;
    }

}
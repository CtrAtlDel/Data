namespace WebApp.Model;

public class UsersReposiroty
{
    public static List<User> db = new List<User>(new[]
    {
        new User()
        {
            Guid = Guid.NewGuid(), Login = "admin", Password = "admin", Name = "admin", Gender = 1, Birthday = null,
            Admin = true,
            CreatedOn = DateTime.Now, CreatedBy = "admin", ModifiedOn = DateTime.Now, ModifiedBy = "admin",
            RevorkedOn = DateTime.Now, RevorkedBy = "admin"
        },

        new User()
        {
            Guid = Guid.NewGuid(), Login = "Ivan", Password = "Ivan", Name = "Ivan", Gender = 1, Birthday = null,
            Admin = false,
            CreatedOn = DateTime.Now, CreatedBy = "admin", ModifiedOn = DateTime.Now, ModifiedBy = "admin",
            RevorkedOn = DateTime.Now, RevorkedBy = ""
        },

        new User()
        {
            Guid = Guid.NewGuid(), Login = "Petr", Password = "Petr", Name = "Petr", Gender = 1, Birthday = null,
            Admin = false,
            CreatedOn = DateTime.Now, CreatedBy = "admin", ModifiedOn = DateTime.Now, ModifiedBy = "admin",
            RevorkedOn = DateTime.Now, RevorkedBy = "admin"
        }
    });
}
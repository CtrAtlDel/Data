using Microsoft.AspNetCore.Mvc;

namespace WebApp.Model;

public class User
{
    public Guid Guid { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public int Gender { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime RevorkedOn { get; set; }
}
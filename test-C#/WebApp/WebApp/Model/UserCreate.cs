namespace WebApp.Model;

public class UserCreate
{
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string Name { get; set; }
    
    public int Gender { get; set; }
    
    public DateTime? Birthday { get; set; }
    
    public bool Admin { get; set; }
}
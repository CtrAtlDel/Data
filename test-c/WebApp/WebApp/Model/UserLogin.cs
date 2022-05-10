using System.ComponentModel.DataAnnotations;

namespace WebApp.Model;

public class Login
{
    [Required(ErrorMessage = "Email Required")]
    public string UserLogin { get; set; }

    [Required(ErrorMessage = "Password Required")]
    public string Password { get; set; }
}
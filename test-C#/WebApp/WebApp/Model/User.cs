using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Model;

public class User
{
    public Guid Guid = Guid.NewGuid();

    [Required]
    
    public string Login { get; set; } //add checkers

    [Required] 
    public string Password { get; set; } //add checkers
    
    public string Name { get; set; }
    
    public int Gender { get; set; } //add genders
    
    public DateTime? Birthday { get; set; }
    
    public bool Admin { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public string ModifiedBy { get; set; }
    
    public DateTime? RevorkedOn { get; set; }

    public string RevorkedBy { get; set; }
}
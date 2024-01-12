namespace SalesCrm.Controllers.ViewModels;

public class UserViewModel
{
    public string? Id { get; set; }

    public List<string>? Roles { get; set; }

    public List<string>? ApplicationRoles { get; set; }

    public string? UserName { get; set; } 
    
    public DateTime Created { get; set; } 
    
    public string? Email { get; set; }
    
    public DateTimeOffset? LockoutEnd { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Controllers.ViewModels;

public class UserViewModel
{
    public string? Id { get; set; }

    public List<string>? Roles { get; set; }

    public List<string>? ApplicationRoles { get; set; }

    [Required(ErrorMessage = "User Name is required")]
    public string? UserName { get; set; }

    public DateTime Created { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }
}

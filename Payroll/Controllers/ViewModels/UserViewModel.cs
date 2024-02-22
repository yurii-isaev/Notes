using System.ComponentModel.DataAnnotations;

namespace Payroll.Controllers.ViewModels;

public class UserViewModel
{
    public string? Id { get; set; }

    public List<string>? Roles { get; set; }

    public List<string>? ApplicationRoles { get; set; }

    [Required]
    public string? UserName { get; set; }

    public DateTime Created { get; set; }

    [Required]
    public string? Email { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }
}

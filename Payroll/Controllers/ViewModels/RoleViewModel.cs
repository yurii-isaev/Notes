using System.ComponentModel.DataAnnotations;

namespace Payroll.Controllers.ViewModels;

public class RoleViewModel
{
  public string? Id { get; set; }

  [
    Required(
      ErrorMessage = "Role name is required."
    ),
    StringLength(
      70,
      MinimumLength = 3,
      ErrorMessage = "Role name can't be minimal 3 letters and longer than 70 letters."
    ),
    RegularExpression(
      @"^[A-Z][a-z]*$",
      ErrorMessage = "Role name must start with a capital letter and all other letters must be lowercase."
    )
  ]
  public string? Name { get; set; }
}

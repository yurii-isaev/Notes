using Microsoft.AspNetCore.Identity;

namespace Payroll.Services.Objects;

public class RoleDto : IdentityRole
{
    public override string? Name { get; set; }
}

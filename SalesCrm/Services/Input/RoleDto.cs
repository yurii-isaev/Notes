using Microsoft.AspNetCore.Identity;

namespace SalesCrm.Services.Input;

public class RoleDto : IdentityRole
{
    public override string? Name { get; set; }
}

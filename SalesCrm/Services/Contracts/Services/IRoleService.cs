using Microsoft.AspNetCore.Identity;

namespace SalesCrm.Services.Contracts.Services;

public interface IRoleService
{
    Task<IEnumerable<IdentityRole>> GetRolesAsync();
    
    Task CreateRoleAsync(IdentityRole role);
    
    Task<IdentityRole> GetRoleNameAsync(string roleId);
    
    Task UpdateRoleAsync(IdentityRole newRole);

    Task DeleteRoleAsync(string id);
}

using Payroll.Services.Objects;

namespace Payroll.Services.Contracts.Services;

public interface IRoleService
{
  Task<IEnumerable<RoleDto>> GetRolesAsync();

  Task CreateRoleAsync(RoleDto role);

  Task<RoleDto> GetRoleNameAsync(string roleId);

  Task UpdateRoleAsync(RoleDto newRole);

  Task DeleteRoleAsync(string roleId);
}

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Exceptions;
using Payroll.Services.Objects;

namespace Payroll.Services;

public class RoleService : IRoleService
{
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IMapper _mapper;

    public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        IQueryable<IdentityRole> list = _roleManager.Roles;
        var roleList = _mapper.Map<IEnumerable<RoleDto>>(list);
        return Task.FromResult(roleList);
    }

    public async Task CreateRoleAsync(RoleDto dto)
    {
        if (dto.Name != null)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(dto.Name);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(dto.Name));
            }
            else
            {
                throw new RoleExistsException("Role already exists");
            }
        }
    }

    // For role edit view
    public async Task<RoleDto> GetRoleNameAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        var dto = _mapper.Map<RoleDto>(role);
        return dto;
    }

    // For role edit view update method
    public async Task UpdateRoleAsync(RoleDto newRole)
    {
        if (newRole.Name != null)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(newRole.Name);

            if (!roleExists)
            {
                var roleToUpdate = await _roleManager.FindByIdAsync(newRole.Id);

                if (roleToUpdate?.Name != null)
                {
                    roleToUpdate.Name = newRole.Name;
                    await _roleManager.UpdateAsync(roleToUpdate);
                }
            }
            else
            {
                throw new RoleExistsException("Role already exists");
            }
        }
    }

    public async Task DeleteRoleAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }
    }
}

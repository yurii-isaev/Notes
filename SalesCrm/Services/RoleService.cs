using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Exceptions;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleService> _logger;
    private readonly IToastNotification _toast;

    public RoleService
    (
        RoleManager<IdentityRole> roleManager,
        ILogger<RoleService> log,
        IToastNotification toastNotification, IMapper mapper
    )
    {
        _roleManager = roleManager;
        _logger = log;
        _toast = toastNotification;
        _mapper = mapper;
    }

    public Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        try
        {
            IQueryable<IdentityRole> list = _roleManager.Roles;
            var roleList = _mapper.Map<IEnumerable<RoleDto>>(list);
            return Task.FromResult(roleList);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while getting a role: " + ex.Message);
            return Task.FromResult<IEnumerable<RoleDto>>(null!);
        }
    }

    public async Task CreateRoleAsync(RoleDto dto)
    {
        try
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
        catch (RoleExistsException ex)
        {
            _logger.LogError("[Create Role]\n" + ex.Message + "\n\n");
            throw new RoleExistsException(ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("[Create Role]\n" + ex.Message + "\n\n");
            throw new HttpRequestException(ex.Message, ex, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<IdentityRole> GetRoleNameAsync(string roleId)
    {
        return (await _roleManager.FindByIdAsync(roleId))!;
    }

    public async Task UpdateRoleAsync(IdentityRole newRole)
    {
        var roleToUpdate = await _roleManager.FindByIdAsync(newRole.Id);

        if (roleToUpdate == null)
        {
            _toast.AddErrorToastMessage("not found");
        }

        roleToUpdate!.Name = newRole.Name;

        try
        {
            var updateResult = await _roleManager.UpdateAsync(roleToUpdate);

            if (updateResult.Succeeded)
            {
                _toast.AddSuccessToastMessage("Role successfully created");
            }
            else
            {
                _toast.AddErrorToastMessage("Error updating role name");
            }
        }

        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Exception occurred during role update");
            _toast.AddErrorToastMessage("Exception occurred during role update");
            throw;
        }
    }

    public async Task DeleteRoleAsync(string id)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(id);

            // Удаляем роль
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    _toast.AddSuccessToastMessage("Role successfully deleted");
                }
                else
                {
                    _toast.AddErrorToastMessage("Error deleting role name");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while deleting role: " + ex.Message);
            _toast.AddErrorToastMessage("Exception deleting role");
            throw;
        }
    }
}

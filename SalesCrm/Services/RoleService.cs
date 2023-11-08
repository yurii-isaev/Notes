using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IToastNotification _toast;
    private readonly ILogger<NewsService> _logger;

    public RoleService
    (
        RoleManager<IdentityRole> roleManager,
        ILogger<NewsService> log,
        IToastNotification toastNotification
    )
    {
        _roleManager = roleManager;
        _logger = log;
        _toast = toastNotification;
    }

    public Task<IEnumerable<IdentityRole>> GetRolesAsync()
    {
        try
        {
            return Task.FromResult<IEnumerable<IdentityRole>>(_roleManager.Roles);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating a role: " + ex.Message);
            return null!;
        }
    }

    public Task CreateRoleAsync(IdentityRole role)
    {
        try
        {
            if (role.Name != null && !_roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(role.Name)).GetAwaiter().GetResult();
            }

            _toast.AddSuccessToastMessage("Role successfully created");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating a role: " + ex.Message);
            _toast.AddErrorToastMessage("Error creating role");
        }

        return Task.CompletedTask;
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

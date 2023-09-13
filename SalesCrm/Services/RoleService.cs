using Microsoft.AspNetCore.Identity;
using NToastNotify;

namespace SalesCrm.Services;

public class RoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IToastNotification _toast;
    private readonly ILogger<NewsService> _logger;

    public RoleService(RoleManager<IdentityRole> roleManager, ILogger<NewsService> log, IToastNotification toastNotification)
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
            // Обработка исключения, например, вывод сообщения об ошибке
            _logger.LogError("An error occurred while creating a role: " + ex.Message);

            // Возвращаем null или другое значение в случае ошибки
            return Task.FromResult<IEnumerable<IdentityRole>>(null!);
        }
    }
    
    public Task<IdentityRole> CreateRoleAsync(IdentityRole role)
    {
        try
        {
            if (role.Name != null && ! _roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(role.Name)).GetAwaiter().GetResult();
            }
            
            _toast.AddSuccessToastMessage("News item successfully created !");
            
        }
        catch (Exception ex)
        {
            // Обработка исключения, например, вывод сообщения об ошибке
            _logger.LogError("An error occurred while creating a role: " + ex.Message);
            _toast.AddErrorToastMessage("Error creating role");
            
            // Возвращаем null или другое значение в случае ошибки
            return Task.FromResult<IdentityRole>(null!);
        }

        return (Task<IdentityRole>) Task.CompletedTask;
    }
}

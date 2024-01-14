using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Utils.Reports;

namespace SalesCrm.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    readonly AuthDbContext _context;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly UserManager<User> _userManager;

    public UserRepository(AuthDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserRole>> GetUserListAsync()
    {
        try
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserRole>();

            foreach (var user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserRole
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Created = user.Created,
                    Email = user.Email,
                    LockoutEnd = user.LockoutEnd,
                    Roles = roles.ToList()
                });
            }

            return userList;
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            throw;
        }
    }

    public async Task BlockUsersAsync(string userId)
    {
        try
        {
            var item = await _context.Users.FirstAsync(user => user.Id == userId);
            item.LockoutEnd = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            throw;
        }
    }

    public async Task UnBlockUsersAsync(string userId)
    {
        try
        {
            var item = await _context.Users.FirstAsync(user => user.Id == userId);
            item.LockoutEnd = null;
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            throw;
        }
    }

    public async Task<UserRole> GetUserByIdAsync(string userId)
    {
        User user = (await _userManager.FindByIdAsync(userId))!;
        IList<string> roles = await _userManager.GetRolesAsync(user);
        IList<IdentityRole> appRoles = await _roleManager.Roles.ToListAsync();

        return new UserRole
        {
            Id = user.Id,
            UserName = user.UserName,
            Created = user.Created,
            Email = user.Email,
            LockoutEnd = user.LockoutEnd,
            Roles = roles.ToList(),
            ApplicationRoles = appRoles.ToList()
        };
    }

    public async Task UpdateUserAsync(UserRole user)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.LockoutEnd = user.LockoutEnd;

                await UpdateUserRole(user.Id!, user.Roles!);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"User with id {user.Id} not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating user.", ex);
        }
    }

    public async Task UpdateUserRole(string userId, List<string> selectedRoleIds)
    {
        // Get the user and their current roles
        var user = await _userManager.FindByIdAsync(userId);
        var currentRoles = await _userManager.GetRolesAsync(user!);

        // Calculate the roles to be added and removed
        var rolesToAdd = selectedRoleIds.Except(currentRoles);
        var rolesToRemove = currentRoles.Except(selectedRoleIds);

        // Add new roles
        await _userManager.AddToRolesAsync(user!, rolesToAdd);

        // Remove deprecated roles
        await _userManager.RemoveFromRolesAsync(user!, rolesToRemove);
    }
}

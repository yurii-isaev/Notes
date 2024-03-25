using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payroll.Domains.Entities;
using Payroll.Domains.Identities;
using Payroll.Services.Contracts.Repositories;
using Payroll.Utils.Reports;

namespace Payroll.DataAccess.Repositories;

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
    var item = await _context.Users.FirstAsync(user => user.Id == userId);
    item.LockoutEnd = DateTime.UtcNow;
    await _context.SaveChangesAsync();
  }

  public async Task UnBlockUsersAsync(string userId)
  {
    var item = await _context.Users.FirstAsync(user => user.Id == userId);
    item.LockoutEnd = null;
    await _context.SaveChangesAsync();
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

  public async Task UpdateUserAsync(UserRole editUser)
  {
    try
    {
      var existingUser = await _context.Users
        .FirstOrDefaultAsync(user => user.Id == editUser.Id) ?? throw new InvalidOperationException();

      existingUser.UserName = editUser.UserName;
      existingUser.Email = editUser.Email;
      existingUser.LockoutEnd = editUser.LockoutEnd;

      await UpdateUserRole(editUser.Id!, editUser.Roles!);
      await _context.SaveChangesAsync();
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

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Utils.Logg;

namespace SalesCrm.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    readonly AuthDbContext _context;
    readonly UserManager<User> _userManager;

    public UserRepository(AuthDbContext context, UserManager<User> userManager)
    {
        _context = context;
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
            var item = await _context.Users.FirstAsync(x => x.Id == userId);
            item.LockoutEnd = DateTime.UtcNow.AddYears(1000);
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
            var item = await _context.Users.FirstAsync(x => x.Id == userId);
            item.LockoutEnd = null;
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            throw;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Services.Contracts;

namespace SalesCrm.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private AuthDbContext _context;

    public UserRepository(AuthDbContext ctx) => _context = ctx;
    
    public async Task<List<IdentityUser>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task BlockUsersAsync(string userId)
    {
        var item = await _context.Users.FirstAsync(x => x.Id == userId);
        item.LockoutEnd = DateTime.UtcNow.AddYears(1000);
        await _context.SaveChangesAsync();
    }

    public async Task UnBlockUsersAsync(string userId)
    {
        var item = await _context.Users.FirstAsync(x => x.Id == userId);
        item.LockoutEnd = null;
        await _context.SaveChangesAsync();
    }
}

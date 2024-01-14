using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserRole>> GetUserListAsync();
    Task BlockUsersAsync(string id);
    Task UnBlockUsersAsync(string id);
    Task<UserRole> GetUserByIdAsync(string userId);
    Task UpdateUserAsync(UserRole user);
}

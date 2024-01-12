using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;
using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserRole>> GetUserListAsync();
    Task BlockUsersAsync(string id);
    Task UnBlockUsersAsync(string id);
    Task<UserRole> GetUserByIdAsync(string userId);
}

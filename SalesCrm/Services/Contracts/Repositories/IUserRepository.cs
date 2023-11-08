using Microsoft.AspNetCore.Identity;

namespace SalesCrm.Services.Contracts.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<IdentityUser>> GetUsersAsync();
    Task BlockUsersAsync(string id);
    Task UnBlockUsersAsync(string id);
}

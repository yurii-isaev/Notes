using Microsoft.AspNetCore.Identity;

namespace SalesCrm.Services.Contracts;

public interface IUserRepository
{
    Task<List<IdentityUser>> GetUsersAsync();
    Task BlockUsersAsync(string id);
    Task UnBlockUsersAsync(string id);
}

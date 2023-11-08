using Microsoft.AspNetCore.Identity;

namespace SalesCrm.Services.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<IdentityUser>> GetUsersAsync();
    
    Task BlockUsersAsync(string id);
    
    Task UnBlockUsersAsync(string id);
}

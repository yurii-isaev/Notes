using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUserListAsync();

    Task BlockUserAsync(string id);

    Task UnBlockUserAsync(string id);
}

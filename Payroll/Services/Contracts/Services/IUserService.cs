using Payroll.Services.Objects;

namespace Payroll.Services.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUserListAsync();

    Task BlockUserAsync(string id);

    Task UnBlockUserAsync(string id);
    
    Task<UserDto> GetUserByIdAsync(string id);

    Task UpdateUserAsync(UserDto dto);
}

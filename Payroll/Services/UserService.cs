using AutoMapper;
using Payroll.Domains.Entities;
using Payroll.Services.Contracts.Repositories;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;

namespace Payroll.Services;

public class UserService : IUserService
{
    readonly IUserRepository _repository;
    readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetUserListAsync()
    {
        var userList = await _repository.GetUserListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(userList);
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);
        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateUserAsync(UserDto dto)
    {
        var user = _mapper.Map<UserRole>(dto);
        await _repository.UpdateUserAsync(user);
    }

    public async Task BlockUserAsync(string userId)
    {
        await _repository.BlockUsersAsync(userId);
    }

    public async Task UnBlockUserAsync(string userId)
    {
        await _repository.UnBlockUsersAsync(userId);
    }
}

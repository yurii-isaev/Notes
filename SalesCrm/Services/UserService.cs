using AutoMapper;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Utils.Reports;

namespace SalesCrm.Services;

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
        try
        {
            var userList = await _repository.GetUserListAsync();
            var userListDto = _mapper.Map<IEnumerable<UserDto>>(userList);
            return userListDto;
        }
        catch (HttpRequestException ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);
        var dto = _mapper.Map<UserDto>(user);
        return dto;
    }

    public async Task UpdateUserAsync(UserDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task BlockUserAsync(string id)
    {
        try
        {
            await _repository.BlockUsersAsync(id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    public async Task UnBlockUserAsync(string id)
    {
        try
        {
            await _repository.UnBlockUsersAsync(id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }
}

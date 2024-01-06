using System.Net;
using AutoMapper;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class UserService : IUserService
{
    readonly IUserRepository _repository;
    readonly ILogger<NewsService> _logger;
    readonly IMapper _mapper;

    public UserService(IUserRepository repository, ILogger<NewsService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
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
            _logger.LogError("[Get Users]\n" + ex.Message + "\n\n");
            throw new HttpRequestException(ex.Message, ex, HttpStatusCode.InternalServerError);
        }
    }

    public async Task BlockUserAsync(string id)
    {
        await _repository.BlockUsersAsync(id);
    }

    public async Task UnBlockUserAsync(string id)
    {
        await _repository.UnBlockUsersAsync(id);
    }
}

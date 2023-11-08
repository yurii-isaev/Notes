using Microsoft.AspNetCore.Identity;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repo)
    {
        _repository = repo;
    }

    public async Task<IEnumerable<IdentityUser>> GetUsersAsync()
    {
        return await _repository.GetUsersAsync();
    }

    public async Task BlockUsersAsync(string id)
    {
        await _repository.BlockUsersAsync(id);
    }

    public async Task UnBlockUsersAsync(string id)
    {
        await _repository.UnBlockUsersAsync(id);
    }
}

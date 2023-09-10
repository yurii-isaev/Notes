using Microsoft.AspNetCore.Identity;
using NToastNotify;
using SalesCrm.Services.Contracts;

namespace SalesCrm.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repo)
    {
        _repository = repo;
    }
    
    public async Task<List<IdentityUser>> GetUsersAsync()
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

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<UserViewModel> list = _userService.GetUsersAsync()
            .Result
            .Select(user => _mapper.Map<UserViewModel>(user))
            .ToList();
        
        return await Task.FromResult<IActionResult>(View(list));
    }

    [Route("/users/block/{id}")]
    public async Task<IActionResult> BlockUsers(string id)
    {
        await _userService.BlockUsersAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [Route("/users/unblock/{id}")]
    public async Task<IActionResult> UnBlockUsers(string id)
    {
        await _userService.UnBlockUsersAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

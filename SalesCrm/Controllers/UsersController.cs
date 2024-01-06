using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    readonly IMapper _mapper;
    readonly IUserService _userService;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("/users")]
    public async Task<IActionResult> Index()
    {
        List<UserViewModel> list = _userService.GetUserListAsync()
            .Result
            .Select(user => _mapper.Map<UserViewModel>(user))
            .ToList();

        return await Task.FromResult<IActionResult>(View(list));
    }

    [Route("/users/block/{id}")]
    public async Task<IActionResult> BlockUser(string id)
    {
        await _userService.BlockUserAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [Route("/users/unblock/{id}")]
    public async Task<IActionResult> UnBlockUser(string id)
    {
        await _userService.UnBlockUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

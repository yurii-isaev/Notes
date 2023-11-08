using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService) => _roleService = roleService;

    public async Task<IActionResult> Index()
    {
        var roleList = await _roleService.GetRolesAsync();
        return View(roleList);
    }

    [HttpGet]
    [Route("/roles/createRole")]
    public Task<IActionResult> CreateRole()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [HttpPost]
    [Route("/roles/create")]
    public async Task<IActionResult> Create(IdentityRole role)
    {
        await _roleService.CreateRoleAsync(role);
        return Redirect("Index");
    }

    [HttpGet]
    [Route("/roles/edit/{id}")]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await _roleService.GetRoleNameAsync(id);
        return View(role);
    }

    [HttpPost]
    [Route("/roles/edit/{id}")]
    public async Task<IActionResult> Edit(IdentityRole role)
    {
        await _roleService.UpdateRoleAsync(role);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/roles/delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        await _roleService.DeleteRoleAsync(id);
        return RedirectToAction("Index");
    }
}

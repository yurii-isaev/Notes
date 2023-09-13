using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Services;

namespace SalesCrm.Controllers;

public class RolesController : Controller
{
    private readonly RoleService _roleService;

    public RolesController(RoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet]
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
    [Route("/roles/createRole")]
    public async Task<IActionResult> Create(IdentityRole role)
    {
        await _roleService.CreateRoleAsync(role);
        return Redirect("Index");
    }
}

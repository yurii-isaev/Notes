using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SalesCrm.Controllers;

public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }
    
    [HttpGet]
    [Route("/roles/createRole")]
    public IActionResult CreateRole()
    {
        return View();
    }
    
    [HttpPost]
    [Route("/roles/createRole")]
    public async Task<IActionResult> Create(IdentityRole role)
    {
        if (role.Name != null && ! _roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(role.Name)).GetAwaiter().GetResult();
        }
        return Redirect("Index");
    }
}

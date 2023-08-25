using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.Contracts;
using SalesCrm.Views.ViewModels;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly INewsService _newsService;

    public AdminController(INewsService newsService)
    {
        _newsService = newsService;
    }

    public IActionResult Index()
    {
        var isAdmin = User.IsInRole("Admin");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Users()
    {
        var listUsers = new List<string>();
        return View(listUsers);
    }

    public async Task<IActionResult> News()
    {
        var newsList = await _newsService.GetNewsAsync();
        return View(newsList);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

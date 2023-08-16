using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;

namespace SalesCrm.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger) => _logger = logger;

    public ActionResult Index()
    {
        var isAdmin = User.IsInRole("Admin");
        _logger.LogWarning(isAdmin.ToString());
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
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
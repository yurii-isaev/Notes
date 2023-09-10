using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.Contracts;
using SalesCrm.Domains.Entities;
using SalesCrm.Services;
using SalesCrm.Views.ViewModels;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly INewsService _newsService;
    private readonly UserService _userService;

    public AdminController(INewsService newsService, UserService userService)
    {
        _newsService = newsService;
        _userService = userService;
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

    public async Task<IActionResult> Users()
    {
        var userList = await _userService.GetUsersAsync();
        return View(userList);
    }

    [Route("/admin/users/block/{id}")]
    public async Task<IActionResult> BlockUsers(string id)
    {
        await _userService.BlockUsersAsync(id);
        return Redirect("/admin/users");
    }

    [Route("/admin/users/unblock/{id}")]
    public async Task<IActionResult> UnBlockUsers(string id)
    {
        await _userService.UnBlockUsersAsync(id);
        return Redirect("/admin/users");
    }

    public async Task<IActionResult> News()
    {
        var newsList = await _newsService.GetNewsAsync();
        return View(newsList);
    }

    [Route("/admin/news/createNews")]
    [HttpGet]
    public Task<IActionResult> CreateNews()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [Route("/admin/news/createNews")]
    [HttpPost]
    public async Task<IActionResult> Create(News news)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId))
        {
            news.AuthorId = userId;
            news.Date = DateTime.SpecifyKind(news.Date, DateTimeKind.Utc);

            await _newsService.CreateNewsAsync(news);
        }

        return Redirect("/admin/news");
    }

    [Route("/admin/news/edit/{id}")]
    [HttpGet]
    public async Task<IActionResult> EditNews(int id)
    {
        var news = await _newsService.GetOneNewsAsync(id);
        return View(news);
    }

    [Route("/admin/news/edit/{id}")]
    [HttpPost]
    public async Task<IActionResult> Edit(News news)
    {
        news.Date = DateTime.SpecifyKind(news.Date, DateTimeKind.Utc);
        await _newsService.UpdateNewsAsync(news);
        return Redirect("/admin/news");
    }

    [Route("/admin/news/delete/{id}")]
    [HttpGet]
    public async Task<IActionResult> DeleteNews(int id)
    {
        await _newsService.DeleteNewsAsync(id);
        return Redirect("/admin/news");
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

using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class NewsController : Controller
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService) => _newsService = newsService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.CurrentPage = "News/Index";
        var newsList = await _newsService.GetNewsListAsync();
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

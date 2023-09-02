﻿using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.Contracts;
using SalesCrm.Domains.Entities;
using SalesCrm.Views.ViewModels;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly INewsService _newsService;

    public AdminController(INewsService service)
    {
        _newsService = service;
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

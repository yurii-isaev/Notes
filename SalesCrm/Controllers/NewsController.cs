using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Views.Components.Pagination;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class NewsController : Controller
{
    private readonly INewsService _newsService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toast;

    public NewsController(INewsService newsService, IMapper mapper, IToastNotification toast)
    {
        _newsService = newsService;
        _mapper = mapper;
        _toast = toast;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 7)
    {
        try
        {
            List<NewsViewModel> list = _newsService.GetNewsListAsync()
                .Result
                .Select(news => _mapper.Map<NewsViewModel>(news))
                .ToList();

            var paginationList = PaginationList<NewsViewModel>.Create(list, pageNumber, pageSize);

            return await Task.FromResult<IActionResult>(View(paginationList));
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    [Route("/admin/news/create")]
    [HttpGet]
    public async Task<IActionResult> CreateNews()
    {
        return await Task.FromResult<IActionResult>(View());
    }

    [Route("/admin/news/create")]
    [HttpPost]
    public async Task<IActionResult> Create(NewsViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (ModelState.IsValid)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    viewModel.AuthorId = userId;
                    viewModel.CreatedAt = DateTime.SpecifyKind(viewModel.CreatedAt, DateTimeKind.Utc);

                    var dto = _mapper.Map<NewsDto>(viewModel);
                    await _newsService.CreateNewsAsync(dto);
                    
                    _toast.AddSuccessToastMessage("News created successfully");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error creating news");
                _toast.AddErrorToastMessage("Error creating news");
            }
        }

        return RedirectToAction("Index");
    }

    [Route("/admin/news/edit/{id}")]
    [HttpGet]
    public async Task<IActionResult> EditNews(Guid id)
    {
        try
        {
            var dto = await _newsService.GetNewsItemAsync(id);
            var viewModel = _mapper.Map<NewsViewModel>(dto);
            return await Task.FromResult<IActionResult>(View(viewModel));
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    [Route("/admin/news/edit/{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(NewsViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var dto = _mapper.Map<NewsDto>(viewModel);
                await _newsService.UpdateNewsAsync(dto);
                
                _toast.AddSuccessToastMessage("News updated successfully");
            }
            catch
            {
                ModelState.AddModelError("", "Error updated news");
                _toast.AddErrorToastMessage("Error updated news");
                return RedirectToAction("Error");
            }
        }

        return RedirectToAction("Index");
    }

    [Route("/admin/news/delete/{id}")]
    [HttpGet]
    public async Task<IActionResult> DeleteNews(Guid id)
    {
        try
        {
            await _newsService.DeleteNewsAsync(id);
            _toast.AddSuccessToastMessage("News deleted successfully");
        }
        catch
        {
            ModelState.AddModelError("", "Error deleted news");
            _toast.AddErrorToastMessage("Error deleted news");
        }
        
        return RedirectToAction("Index");
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

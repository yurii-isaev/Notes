using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Payroll.Controllers.ViewModels;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;
using Payroll.Utils.Reports;

namespace Payroll.Pages.News;

[Authorize(Roles = "Admin, Manager")]
public class EditModel : PageModel
{
    readonly INewsService _newsService;
    readonly IMapper _mapper;
    readonly IToastNotification _toast;

    public EditModel(INewsService service, IMapper mapper, IToastNotification toast)
    {
        _newsService = service;
        _mapper = mapper;
        _toast = toast;
    }

    [BindProperty(Name = "newsId")]
    public Guid NewsId { get; set; }
    
    [BindProperty]
    public NewsViewModel News { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid newsId)
    {
        try
        {
            var news = await _newsService.GetNewsItemAsync(newsId);

            News = new NewsViewModel()
            {
                Title = news.Title,
                Description = news.Description,
                CreatedAt = news.CreatedAt,
                PublishedAt = news.PublishedAt
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return NotFound();
        }
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            try
            {
                var existingNews = await _newsService.GetNewsItemAsync(NewsId);
                News.CreatedAt = existingNews.CreatedAt;
                News.PublishedAt = existingNews.PublishedAt;
                News.UpdatedAt = DateTime.UtcNow;
                News.Id = NewsId;
                var dto = _mapper.Map<NewsDto>(News);
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

        return Page();
    }
}

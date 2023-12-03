using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Pages.News;

[Authorize(Roles = "Manager")]
public class EditModel : PageModel
{
    private readonly INewsService _newsService;
    private readonly ILogger<EditModel> _logger;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toast;

    public EditModel(INewsService service, ILogger<EditModel> logger, IMapper mapper, IToastNotification toast)
    {
        _newsService = service;
        _logger = logger;
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
            _logger.LogError("[EditModel .. On Get Async]: " + ex.Message);
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

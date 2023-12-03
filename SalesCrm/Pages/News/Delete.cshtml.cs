using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Pages.News;

public class DeleteModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly INewsService _newsService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toast;

    public DeleteModel(ILogger<IndexModel> logger, INewsService service, IMapper mapper, IToastNotification toast)
    {
        _logger = logger;
        _newsService = service;
        _mapper = mapper;
        _toast = toast;
    }

    [BindProperty(Name = "newsId")] public Guid NewsId { get; set; }

    [BindProperty] public IEnumerable<NewsViewModel> NewsList { get; set; } = null!;


    public async Task OnGetAsync()
    {
        try
        {
            var news = await _newsService.GetOnlyActiveNewsAsync();
            var viewModel = _mapper.Map<IEnumerable<NewsViewModel>>(news);
            NewsList = viewModel;
        }
        catch (Exception)
        {
            RedirectToAction("/Error");
        }
    }
    
    
    public async Task<IActionResult> OnPost(Guid id)
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
            RedirectToPage("/Error");
        }

        return RedirectToPage("/Error");
    }
}

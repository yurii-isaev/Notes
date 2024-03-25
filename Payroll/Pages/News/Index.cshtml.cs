using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Payroll.Controllers.ViewModels;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;
using Payroll.Utils.Reports;

namespace Payroll.Pages.News
{
  [Authorize(Roles = "Admin, Manager")]
  public class IndexModel : PageModel
  {
    readonly INewsService _newsService;
    readonly IMapper _mapper;
    readonly IToastNotification _toast;

    public IndexModel(INewsService service, IMapper mapper, IToastNotification toast)
    {
      _newsService = service;
      _mapper = mapper;
      _toast = toast;
    }

    [BindProperty(Name = "newsId")] public Guid NewsId { get; set; }
    [BindProperty] public IEnumerable<NewsViewModel> NewsList { get; set; } = null!;
    [BindProperty] public int TotalItemsCount { get; set; }
    [BindProperty] public int PageSize { get; set; }
    [BindProperty] public int PageNumber { get; set; }
    [BindProperty] public int TotalPages { get; set; }
    [BindProperty] public bool IsPreviousPageAvailable { get; set; }
    [BindProperty] public bool IsNextPageAvailable { get; set; }

    public async Task OnGetAsync(string keyword, int pageNumber = 1, int pageSize = 4)
    {
      IEnumerable<NewsDto> news = await _newsService.GetOnlyActiveNewsAsync(keyword);
      IEnumerable<NewsDto> newsList = news.ToList();
      List<NewsViewModel> list = newsList
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(item => _mapper.Map<NewsViewModel>(item))
        .ToList();

      NewsList = list;

      // Set values for pagination properties
      TotalItemsCount = newsList.Count();
      PageSize = pageSize;
      PageNumber = pageNumber;
      TotalPages = (int) Math.Ceiling((double) TotalItemsCount / PageSize);
      IsPreviousPageAvailable = PageNumber > 1;
      IsNextPageAvailable = PageNumber < TotalPages;
    }

    public async Task<IActionResult> OnPost(Guid id)
    {
      try
      {
        await _newsService.DeleteNewsAsync(id);
        _toast.AddSuccessToastMessage("News deleted successfully");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
        _toast.AddErrorToastMessage("Error deleted news");
        return RedirectToPage("Error", new {errorMessage = ex.Message});
      }

      return RedirectToPage("Index");
    }
  }
}

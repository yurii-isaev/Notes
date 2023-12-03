using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toast;

        public IndexModel(ILogger<IndexModel> logger, INewsService service, IMapper mapper, IToastNotification toast)
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
            catch (Exception ex)
            {
                RedirectToPage("Error", new { errorMessage = ex.Message });
            }
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> OnPost(Guid id)
        {
            try
            {
                await _newsService.DeleteNewsAsync(id);
                _toast.AddSuccessToastMessage("News deleted successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleted news");
                _toast.AddErrorToastMessage("Error deleted news");
                
                return RedirectToPage("Error", new { errorMessage = ex.Message });
            }
            
            return Page();
        }
    }
}

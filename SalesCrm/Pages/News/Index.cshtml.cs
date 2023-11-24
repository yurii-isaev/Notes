using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public IndexModel(ILogger<IndexModel> logger, INewsService service, IMapper mapper)
        {
            _logger = logger;
            _newsService = service;
            _mapper = mapper;
        }

        [BindProperty]
        public IEnumerable<NewsViewModel> NewsList { get; set; } = null!;

        
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
                RedirectToAction("Error");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;

namespace SalesCrm.Pages
{
    public class NewsModel : PageModel
    {
        private readonly ILogger<NewsModel> _logger;
        private readonly INewsRepository _repository;

        public NewsModel(ILogger<NewsModel> logger, INewsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<News> News { get; set; } = null!;

        public async Task OnGetAsync()
        {
            News = await _repository.GetOnlyActiveNewsAsync();
        }
    }
}

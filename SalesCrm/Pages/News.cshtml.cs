using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;

namespace SalesCrm.Pages
{
    public class NewsModel : PageModel
    {
        private readonly ILogger<NewsModel> _logger;
        private readonly IDataRepository<News> _repository;

        public NewsModel(ILogger<NewsModel> logger, IDataRepository<News> repository)
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

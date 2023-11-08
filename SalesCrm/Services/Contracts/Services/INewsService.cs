using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Services;

public interface INewsService
{
    Task<IEnumerable<News>> GetNewsListAsync();
    
    Task<News> CreateNewsAsync(News news);
    
    Task<News> GetOneNewsAsync(int id);
    
    Task<News> UpdateNewsAsync(News news);
    
    Task DeleteNewsAsync(int id);
}

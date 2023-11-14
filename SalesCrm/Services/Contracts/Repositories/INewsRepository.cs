using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Repositories;

public interface INewsRepository
{
    Task<IEnumerable<News>> GetNewsListAsync();
    
    Task<IEnumerable<News>> GetOnlyActiveNewsAsync();
    
    Task CreateNewsAsync(News news);
    
    Task<News> GetOneNewsAsync(int id);
    
    Task<News> UpdateNewsAsync(News news);
    
    Task DeleteNewsAsync(int id);
}

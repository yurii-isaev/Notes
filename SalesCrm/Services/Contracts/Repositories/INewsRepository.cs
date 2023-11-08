using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Repositories;

public interface INewsRepository
{
    Task<IEnumerable<News>> GetNewsListAsync();
    
    Task<IEnumerable<News>> GetOnlyActiveNewsAsync();
    
    Task<News> CreateNewsAsync(News type);
    
    Task<News> GetOneNewsAsync(int id);
    
    Task<News> UpdateNewsAsync(News type);
    
    Task DeleteNewsAsync(int id);
}

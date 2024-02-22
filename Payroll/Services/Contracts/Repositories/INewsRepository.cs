using Payroll.Domains.Entities;

namespace Payroll.Services.Contracts.Repositories;

public interface INewsRepository
{
    Task<IEnumerable<News>> GetNewsListAsync();
    
    Task<IEnumerable<News>> GetOnlyActiveNewsListAsync();
    
    Task CreateNewsAsync(News news);
    
    Task<News> GetOneNewsAsync(Guid newsId);
    
    Task UpdateNewsAsync(News news);
    
    Task DeleteNewsAsync(Guid newsId);
}

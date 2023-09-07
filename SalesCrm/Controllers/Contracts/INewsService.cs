using SalesCrm.Domains.Entities;

namespace SalesCrm.Controllers.Contracts;

public interface INewsService
{
    Task<IEnumerable<News>> GetNewsAsync();
    Task<News> CreateNewsAsync(News news);
    Task<News> GetOneNewsAsync(int id);
    Task<News> UpdateNewsAsync(News news);
    Task DeleteNewsAsync(int id);
}

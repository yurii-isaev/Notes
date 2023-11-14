using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface INewsService
{
    Task<IEnumerable<NewsDto>> GetNewsListAsync();

    Task CreateNewsAsync(NewsDto dto);

    Task<NewsDto> GetNewsItemAsync(int id);

    Task UpdateNewsAsync(NewsDto dto);

    Task DeleteNewsAsync(int id);
}

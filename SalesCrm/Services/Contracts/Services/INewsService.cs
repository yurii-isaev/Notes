using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface INewsService
{
    Task<IEnumerable<NewsDto>> GetNewsListAsync();

    Task CreateNewsAsync(NewsDto dto);

    Task<NewsDto> GetNewsItemAsync(Guid id);

    Task UpdateNewsAsync(NewsDto dto);

    Task DeleteNewsAsync(Guid id);
}

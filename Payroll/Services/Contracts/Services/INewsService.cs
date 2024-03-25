using Payroll.Services.Objects;

namespace Payroll.Services.Contracts.Services;

public interface INewsService
{
  Task<IEnumerable<NewsDto>> GetNewsListAsync();

  Task CreateNewsAsync(NewsDto newsDto);

  Task<NewsDto> GetNewsItemAsync(Guid newsId);

  Task<IEnumerable<NewsDto>> GetOnlyActiveNewsAsync(string keyword);

  Task UpdateNewsAsync(NewsDto newsDto);

  Task DeleteNewsAsync(Guid newsId);
}

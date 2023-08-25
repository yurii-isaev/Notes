using SalesCrm.Controllers.Contracts;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;

namespace SalesCrm.Services;

public class NewsService : INewsService
{
    private readonly IDataRepository<News> _repository;
    private readonly ILogger<NewsService> _logger;

    public NewsService(IDataRepository<News> repo, ILogger<NewsService> log)
    {
        _repository = repo;
        _logger = log;
    }

    public async Task<IEnumerable<News>> GetNewsAsync()
    {
        try
        {
            return await _repository.GetAsync();
        }
        catch (Exception ex)
        {
            // Обработка исключения, например, логирование или возврат значения по умолчанию
            // Можно здесь выполнить логирование ошибки
            _logger.LogWarning(ex.Message);

            // Возврат значения по умолчанию или пустой коллекции в случае ошибки
            return Enumerable.Empty<News>();
        }
    }
}

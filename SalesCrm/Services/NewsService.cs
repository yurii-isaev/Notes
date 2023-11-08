using NToastNotify;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _repository;
    private readonly ILogger<NewsService> _logger;
    private readonly IToastNotification _toast;

    public NewsService(INewsRepository repo, ILogger<NewsService> log, IToastNotification toastNotification)
    {
        _repository = repo;
        _logger = log;
        _toast = toastNotification;
    }

    public async Task<IEnumerable<News>> GetNewsListAsync()
    {
        try
        {
            return await _repository.GetNewsListAsync();
        }
        catch (Exception ex)
        {
            // Обработка исключения, например, логирование или возврат значения по умолчанию
            // Можно здесь выполнить логирование ошибки
            _logger.LogError(ex.Message);

            // Возврат значения по умолчанию или пустой коллекции в случае ошибки
            return Enumerable.Empty<News>();
        }
    }

    public async Task<News> CreateNewsAsync(News news)
    {
        try
        {
            _toast.AddSuccessToastMessage("News item successfully created !");
            return await _repository.CreateNewsAsync(news);
        }
        catch (Exception ex)
        {
            // Обработка исключения, например, вывод сообщения об ошибке
            _logger.LogError("An error occurred while creating a news item: " + ex.Message);
            _toast.AddErrorToastMessage("Error creating news item");
            
            // Возвращаем null или другое значение в случае ошибки
            return null!;
        }
    }

    public async Task<News> GetOneNewsAsync(int id)
    {
        return await _repository.GetOneNewsAsync(id);
    }

    public async Task<News> UpdateNewsAsync(News news)
    {
        try
        {
            _toast.AddSuccessToastMessage("News item successfully updated !");
            return await _repository.UpdateNewsAsync(news);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while updating news: " + ex.Message);
            _toast.AddErrorToastMessage("Error updating news item");
            throw;
        }
    }

    public async Task DeleteNewsAsync(int id)
    {
        try
        {
            await _repository.DeleteNewsAsync(id);
            _toast.AddSuccessToastMessage("News item successfully deleted !");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while deleting news: " + ex.Message);
            _toast.AddErrorToastMessage("Error deleting news item");
            throw;
        }
    }
}

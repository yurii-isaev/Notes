using AutoMapper;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _repository;
    private readonly ILogger<NewsService> _logger;
    private readonly IMapper _mapper;

    public NewsService(INewsRepository repository, ILogger<NewsService> log, IMapper mapper)
    {
        _repository = repository;
        _logger = log;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NewsDto>> GetNewsListAsync()
    {
        try
        {
            var newsList = await _repository.GetNewsListAsync();
            var newsTransferList = _mapper.Map<IEnumerable<NewsDto>>(newsList);

            return newsTransferList;
        }
        catch (Exception ex)
        {
            _logger.LogError("[NewsService .. Get News List]: " + ex.Message);
            return Enumerable.Empty<NewsDto>();
        }
    }

    public async Task CreateNewsAsync(NewsDto dto)
    {
        try
        {
            var entity = _mapper.Map<News>(dto);
            await _repository.CreateNewsAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NewsService .. Create News]: " + ex.Message);
            throw;
        }
    }

    public async Task<NewsDto> GetNewsItemAsync(Guid id)
    {
        try
        {
            var news = await _repository.GetOneNewsAsync(id);
            var dto = _mapper.Map<NewsDto>(news);
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError("[NewsService .. Get News Item]: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateNewsAsync(NewsDto dto)
    {
        try
        {
            var news = _mapper.Map<News>(dto);
            await _repository.UpdateNewsAsync(news);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NewsService .. Update News]: " + ex.Message);
            throw;
        }
    }

    public async Task DeleteNewsAsync(Guid id)
    {
        try
        {
            await _repository.DeleteNewsAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NewsService .. Delete News]: " + ex.Message);
            throw;
        }
    }
}

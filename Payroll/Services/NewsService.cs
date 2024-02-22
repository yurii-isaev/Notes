using AutoMapper;
using Payroll.Domains.Entities;
using Payroll.Services.Contracts.Repositories;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;

namespace Payroll.Services;

public class NewsService : INewsService
{
    readonly INewsRepository _repository;
    readonly IMapper _mapper;

    public NewsService(INewsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NewsDto>> GetNewsListAsync()
    {
        try
        {
            var newsList = await _repository.GetNewsListAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(newsList);
        }
        catch (Exception)
        {
            return Enumerable.Empty<NewsDto>();
        }
    }

    public async Task CreateNewsAsync(NewsDto dto)
    {
        var entity = _mapper.Map<News>(dto);
        await _repository.CreateNewsAsync(entity);
    }

    public async Task<NewsDto> GetNewsItemAsync(Guid newsId)
    {
        var news = await _repository.GetOneNewsAsync(newsId);
        return _mapper.Map<NewsDto>(news);
    }

    public async Task<IEnumerable<NewsDto>> GetOnlyActiveNewsAsync(string keyword)
    {
        var newsList = await _repository.GetOnlyActiveNewsListAsync();

        if (!string.IsNullOrEmpty(keyword))
        {
            newsList = newsList.Where(emp => emp.Title!.Contains(keyword));
        }

        return _mapper.Map<IEnumerable<NewsDto>>(newsList);
    }

    public async Task UpdateNewsAsync(NewsDto dto)
    {
        var news = _mapper.Map<News>(dto);
        await _repository.UpdateNewsAsync(news);
    }

    public async Task DeleteNewsAsync(Guid id)
    {
        await _repository.DeleteNewsAsync(id);
    }
}

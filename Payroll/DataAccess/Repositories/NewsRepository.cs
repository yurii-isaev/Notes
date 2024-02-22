using Microsoft.EntityFrameworkCore;
using Payroll.Domains.Entities;
using Payroll.Services.Contracts.Repositories;

namespace Payroll.DataAccess.Repositories;

public class NewsRepository : INewsRepository
{
    readonly NewsDbContext _context;

    public NewsRepository(NewsDbContext context) => _context = context;

    public async Task<IEnumerable<News>> GetNewsListAsync()
    {
        return await _context.News
            .Include(n => n.Author)
            .ToListAsync() ?? throw new InvalidOperationException();
    }

    public async Task<IEnumerable<News>> GetOnlyActiveNewsListAsync()
    {
        return await _context.News
            .Where(n => n.IsActive)
            .ToListAsync() ?? throw new InvalidOperationException();
    }

    public async Task CreateNewsAsync(News news)
    {
        await _context.News.AddAsync(news);
        await _context.SaveChangesAsync();
    }

    public async Task<News> GetOneNewsAsync(Guid newsId)
    {
        return await _context.News
            .Where(n => n.Id == newsId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public async Task UpdateNewsAsync(News news)
    {
        var currentNews = await _context.News
            .Where(n => n.Id == news.Id)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();

        currentNews.Title = news.Title;
        currentNews.Description = news.Description;
        currentNews.PublishedAt = news.PublishedAt;
        currentNews.CreatedAt = news.CreatedAt;
        currentNews.UpdatedAt = news.UpdatedAt;
        currentNews.IsActive = news.IsActive;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteNewsAsync(Guid newsId)
    {
        News item = await _context.News
            .Where(n => n.Id == newsId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();

        _context.News.Remove(item);
        await _context.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;

namespace SalesCrm.DataAccess.Repositories;

public class NewsRepository : INewsRepository
{
    private NewsDbContext _context;

    public NewsRepository(NewsDbContext ctx) => _context = ctx;

    public async Task<IEnumerable<News>> GetNewsListAsync()
    {
        return await _context.News.Include(n => n.Author).ToListAsync();
    }

    public async Task<IEnumerable<News>> GetOnlyActiveNewsAsync()
    {
        return await _context.News.Where(n => n.IsActive).ToListAsync();
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
        var currentNews = await _context.News.Where(n => n.Id == news.Id).FirstOrDefaultAsync();

        currentNews!.Title = news.Title;
        currentNews.Description = news.Description;
        currentNews.PublishedAt = news.PublishedAt;
        currentNews.CreatedAt = news.CreatedAt;
        currentNews.UpdatedAt = news.UpdatedAt;
        currentNews.IsActive = news.IsActive;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteNewsAsync(Guid newsId)
    {
        var item = await _context.News.Where(n => n.Id == newsId).FirstOrDefaultAsync();
        _context.News.Remove(item ?? throw new InvalidOperationException());
        await _context.SaveChangesAsync();
    }
}

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

    public async Task<News> UpdateNewsAsync(News news)
    {
        var item = await _context.News.Where(n => n.Id == news.Id).FirstOrDefaultAsync();

        item!.Title = news.Title;
        item.Description = news.Description;
        item.PublishedAt = news.PublishedAt;
        item.IsActive = news.IsActive;

        await _context.SaveChangesAsync();

        return item;
    }

    public async Task DeleteNewsAsync(Guid newsId)
    {
        var item = await _context.News.Where(n => n.Id == newsId).FirstOrDefaultAsync();
        _context.News.Remove(item ?? throw new InvalidOperationException());
        await _context.SaveChangesAsync();
    }
}

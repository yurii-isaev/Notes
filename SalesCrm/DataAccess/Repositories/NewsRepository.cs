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
        return await _context.News.ToListAsync();
    }

    public async Task<IEnumerable<News>> GetOnlyActiveNewsAsync()
    {
        return await _context.News.Where(x => x.IsActive).ToListAsync();
    }

    public async Task<News> CreateNewsAsync(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();
        return news;
    }

    public async Task<News> GetOneNewsAsync(int id)
    {
        return await _context.News
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public async Task<News> UpdateNewsAsync(News news)
    {
        var item = await _context.News.Where(x => x.Id == news.Id).FirstOrDefaultAsync();

        item!.Title = news.Title;
        item.Text = news.Text;
        item.Date = news.Date;
        item.IsActive = news.IsActive;

        await _context.SaveChangesAsync();

        return item;
    }

    public async Task DeleteNewsAsync(int id)
    {
        var item = await _context.News.Where(x => x.Id == id).FirstOrDefaultAsync();
        _context.News.Remove(item ?? throw new InvalidOperationException());
        await _context.SaveChangesAsync();
    }
}

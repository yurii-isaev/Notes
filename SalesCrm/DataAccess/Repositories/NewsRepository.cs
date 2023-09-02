using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;

namespace SalesCrm.DataAccess.Repositories;

public class NewsRepository : IDataRepository<News>
{
    private NewsDbContext _context;

    public NewsRepository(NewsDbContext ctx) => _context = ctx;

    public async Task<IEnumerable<News>> GetAsync()
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
}

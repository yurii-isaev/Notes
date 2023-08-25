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
}

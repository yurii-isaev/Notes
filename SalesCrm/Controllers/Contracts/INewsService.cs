using SalesCrm.Domains.Entities;

namespace SalesCrm.Controllers.Contracts;

public interface INewsService
{
    Task<IEnumerable<News>> GetNewsAsync();
}

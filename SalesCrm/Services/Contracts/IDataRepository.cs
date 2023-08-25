using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts;

public interface IDataRepository<T>
{
    Task<IEnumerable<News>> GetAsync();
}

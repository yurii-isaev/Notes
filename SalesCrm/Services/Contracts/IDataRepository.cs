using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts;

public interface IDataRepository<T>
{
    Task<IEnumerable<T>> GetAsync();
    Task<IEnumerable<T>> GetOnlyActiveNewsAsync();
    Task<T> CreateNewsAsync(T type);
}

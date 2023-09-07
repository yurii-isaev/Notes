namespace SalesCrm.Services.Contracts;

public interface IDataRepository<T>
{
    Task<IEnumerable<T>> GetNewsListAsync();
    Task<IEnumerable<T>> GetOnlyActiveNewsAsync();
    Task<T> CreateNewsAsync(T type);
    Task<T> GetOneNewsAsync(int id);
    Task<T> UpdateNewsAsync(T type);
    Task DeleteNewsAsync(int id);
}

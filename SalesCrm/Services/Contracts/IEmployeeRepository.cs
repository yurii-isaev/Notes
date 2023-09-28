using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeeListAsync();
    Task<Employee> CreateEmployeeAsync(Employee employee);
}

using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts;

public interface IEmployeeRepository
{
    Task<Employee> CreateEmployeeAsync(Employee employee);
}

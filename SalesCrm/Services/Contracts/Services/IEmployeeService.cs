using SalesCrm.Domains.Entities;
using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(EmployeeInputDto employee);
    Task<IEnumerable<Employee>> GetEmployeeListAsync();
}

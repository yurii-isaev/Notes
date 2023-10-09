using SalesCrm.Domains.Entities;
using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(EmployeeDto employee);
    Task<IEnumerable<Employee>> GetEmployeeListAsync();
    Task<EmployeeDto> GetEmployeeByIdAsync(Guid id);
    Task UpdateEmployeeAsync(EmployeeDto employee);
}

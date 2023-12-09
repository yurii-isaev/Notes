using SalesCrm.Domains.Entities;
using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(EmployeeDto employeeDto);
    
    Task<IEnumerable<EmployeeDto>> GetEmployeeListAsync();
    
    Task<IEnumerable<EmployeeDto>> GetEmployeeListAsync(string keyword);
    
    Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId);
    
    Task UpdateEmployeeAsync(EmployeeDto employeeDto);
    
    Task DeleteEmployeeByIdAsync(Guid employeeId);
    
    Task<decimal> GetUnionFree(Guid id);
}

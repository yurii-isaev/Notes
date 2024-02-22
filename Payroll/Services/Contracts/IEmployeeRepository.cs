using Payroll.Domains.Entities;

namespace Payroll.Services.Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeeListAsync();
    
    Task CreateEmployeeAsync(Employee employee);
    
    Task<Employee> GetEmployeeByIdAsync(Guid id);
    
    Task UpdateEmployeeAsync(Employee employee);
    
    Task DeleteEmployeeAsync(Guid employeeId);

    Task<bool> EmployeeNameExistsAsync(string employeeName);
}

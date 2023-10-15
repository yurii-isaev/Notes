using Microsoft.EntityFrameworkCore.ChangeTracking;
using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeeListAsync();
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Employee> GetEmployeeByIdAsync(Guid id);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(Guid employeeId);
}

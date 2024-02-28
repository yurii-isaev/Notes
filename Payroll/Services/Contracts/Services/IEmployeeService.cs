using Payroll.Services.Objects;

namespace Payroll.Services.Contracts.Services;

public interface IEmployeeService
{
    Task CreateEmployeeAsync(EmployeeDto employeeDto);

    Task<IEnumerable<EmployeeDto>> GetEmployeeListAsync(string keyword);

    Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId);

    Task UpdateEmployeeAsync(EmployeeDto employeeDto);

    Task DeleteEmployeeByIdAsync(Guid employeeId);
}

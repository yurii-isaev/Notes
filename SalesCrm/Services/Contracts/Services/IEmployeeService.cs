using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface IEmployeeService
{
    Task CreateEmployeeAsync(EmployeeDto employeeDto);

    Task<IEnumerable<EmployeeDto>> GetEmployeeListAsync(string keyword);

    Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId);

    Task UpdateEmployeeAsync(EmployeeDto employeeDto);

    Task DeleteEmployeeByIdAsync(Guid employeeId);

    Task<decimal> GetUnionFree(Guid id);
}

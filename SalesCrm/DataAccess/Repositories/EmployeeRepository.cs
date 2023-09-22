using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;

namespace SalesCrm.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext ctx) => _context = ctx;

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
}

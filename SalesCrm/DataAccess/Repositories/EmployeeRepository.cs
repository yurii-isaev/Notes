using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;

namespace SalesCrm.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext ctx) => _context = ctx;

    public async Task<IEnumerable<Employee>> GetEmployeeListAsync()
    {
        return await _context.Employees.AsNoTracking().OrderBy(n => n.Name).ToListAsync();
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public Task<Employee> GetEmployeeByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}

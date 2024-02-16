using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;

namespace SalesCrm.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    readonly EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext context) => _context = context;

    public async Task<IEnumerable<Employee>> GetEmployeeListAsync()
    {
        return await _context.Employees.AsNoTracking()
            .OrderBy(n => n.Name)
            .ToListAsync() ?? throw new InvalidOperationException();
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    // Use. AsNoTracking() in this case indicates that you do not plan to modify the received objects
    // and save them back to the database.
    // This can be useful if you're only reading data from a database for display or analysis,
    // and you don't need to track changes or save updates.
    // Possible error -> AsNoTracking()
    // The instance of entity type 'Employee' cannot be tracked because another instance
    // with the same key value for {'Id'} is already being tracked.
    // When attaching existing entities, ensure that only one entity instance with a given key value is attached.
    public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
    {
        // waiting return async-type awaitable,
        // Employee - sync-type,
        // Task.FromResult(employee) - async-type
        return await _context.Employees.AsNoTracking()
            .Where(emp => emp.Id == employeeId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var employee = await _context.Employees
            .Where(emp => emp.Id == employeeId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> EmployeeNameExistsAsync(string employeeName)
    {
        // Convert the name to lowercase for a more reliable comparison (case neutral)
        string lowerEmployeeName = employeeName.ToLower();

        // Check if there is an employee with the specified name in the database
        bool employeeExists = await _context.Employees
            .AnyAsync(e => e.Name!.ToLower() == lowerEmployeeName);

        return employeeExists;
    }
}

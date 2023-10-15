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

    public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
    { 
        // waiting return async-type awaitable,
        // Employee - sync-type,
        // Task.FromResult(employee) - async-type
        return (await _context.Employees.Where(emp => emp.Id == employeeId).FirstOrDefaultAsync())!;
    }
   
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var employee = await _context.Employees.Where(emp => emp.Id == employeeId).FirstOrDefaultAsync();
        _context.Employees.Remove(employee ?? throw new InvalidOperationException());
        await _context.SaveChangesAsync();
    }
}

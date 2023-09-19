using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;

namespace SalesCrm.DataAccess;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {}

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<PaymentRecord> PaymentRecords => Set<PaymentRecord>();
    public DbSet<TaxYear> TaxYears => Set<TaxYear>();
}

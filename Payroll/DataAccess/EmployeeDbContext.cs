using Microsoft.EntityFrameworkCore;
using Payroll.Domains.Entities;

namespace Payroll.DataAccess;

public class EmployeeDbContext : DbContext
{
  public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
  }

  public DbSet<Employee> Employees => Set<Employee>();
  public DbSet<PaymentRecord> PaymentRecords => Set<PaymentRecord>();
  public DbSet<TaxYear> TaxYears => Set<TaxYear>();
}

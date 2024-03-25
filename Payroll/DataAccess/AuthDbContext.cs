using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payroll.Domains.Entities;
using Payroll.Domains.Identities;

namespace Payroll.DataAccess;

public class AuthDbContext : IdentityDbContext
{
  public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>()
      .Property(e => e.Created)
      .HasDefaultValueSql("now()");

    modelBuilder.Entity<News>()
      .HasOne<User>()
      .WithMany()
      .HasForeignKey(n => n.AuthorId);
  }
}

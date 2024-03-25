using Microsoft.EntityFrameworkCore;
using Payroll.Domains.Entities;
using Payroll.Domains.Identities;

namespace Payroll.DataAccess;

public class NewsDbContext : DbContext
{
  public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
  }

  public DbSet<News> News => Set<News>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder
      .Entity<User>()
      .ToTable("AspNetUsers");

    modelBuilder
      .Entity<News>()
      .Property(e => e.PublishedAt)
      .HasDefaultValueSql("now()");

    modelBuilder
      .Entity<News>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql("now()");

    modelBuilder
      .Entity<News>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql("now()");

    modelBuilder
      .Entity<News>()
      .Property(e => e.IsActive)
      .HasDefaultValue(true);
  }
}

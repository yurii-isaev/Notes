using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;

namespace SalesCrm.DataAccess;

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

        modelBuilder.Entity<User>().ToTable("AspNetUsers");

        modelBuilder
            .Entity<News>()
            .Property(e => e.Date)
            .HasDefaultValueSql("now()");

        modelBuilder
            .Entity<News>()
            .Property(e => e.CreateDate)
            .HasDefaultValueSql("now()");

        modelBuilder
            .Entity<News>()
            .Property(e => e.IsActive)
            .HasDefaultValue(true);
    }
}

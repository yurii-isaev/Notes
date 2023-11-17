using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;

namespace SalesCrm.DataAccess;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<IdentityUser>()
        //     .Property(e => e.Created)
        //     .HasDefaultValueSql("now()");
        
        // Configure additional properties for AspNetUsers table
        // modelBuilder.Entity<IdentityUser>()
        //     .Property(u => u.Created)
        //     .HasColumnType("datetime");
        
        modelBuilder.Entity<User>()
            .Property(e => e.Created)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<News>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.AuthorId);
    }
}


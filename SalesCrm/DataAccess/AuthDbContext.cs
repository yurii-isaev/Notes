using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Identities;

namespace SalesCrm.DataAccess;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {}
    
    // public DbSet<User> News => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder
            .Entity<User>()
            .Property(e => e.Created)
            .HasDefaultValueSql("now()");
    }
}

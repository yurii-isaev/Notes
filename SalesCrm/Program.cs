using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Controllers.Contracts;
using SalesCrm.DataAccess;
using SalesCrm.DataAccess.Repositories;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;
using SalesCrm.Services;
using SalesCrm.Services.Contracts;

namespace SalesCrm;

public class Program
{
    public static void Main(string[] args)
    {
        #region Web Application config
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection") ?? throw
            new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<AuthDbContext>(opts => opts.UseNpgsql(connectionString));
        builder.Services.AddDbContext<NewsDbContext>(opts => opts.UseNpgsql(connectionString));
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddTransient<INewsService, NewsService>();
        builder.Services.AddTransient<IDataRepository<News>, NewsRepository>();

        builder.Services
            .AddDefaultIdentity<User>(opts => opts.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();
        #endregion

        #region Configure the HTTP request pipeline
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        app.MapRazorPages();

        app.Run();
        #endregion
    }
}

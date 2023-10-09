using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SalesCrm.Controllers.Contracts;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.DataAccess;
using SalesCrm.DataAccess.Repositories;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;
using SalesCrm.Services;
using SalesCrm.Services.Contracts;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Services.Mapping;

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
        builder.Services.AddDbContext<EmployeeDbContext>(opts => opts.UseNpgsql(connectionString));
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddTransient<INewsService, NewsService>();
        builder.Services.AddTransient<IDataRepository<News>, NewsRepository>();
        
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        
        builder.Services.AddTransient<UserService>();
        builder.Services.AddTransient<RoleService>();

        builder.Services
            .AddDefaultIdentity<User>(opts => opts.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

        builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
        {
            ProgressBar = true,
            PositionClass = ToastPositions.TopRight
        });
        
  
        builder.Services.AddAutoMapper(config =>
            {
                config.CreateMap<EmployeeViewModel, Employee>();
                config.CreateMap<Employee, EmployeeViewModel>();

                config.CreateMap<EmployeeViewModel, EmployeeDto>();
                config.CreateMap<EmployeeDto, EmployeeViewModel>();

                config.CreateMap<EmployeeDto, Employee>();
                config.CreateMap<Employee, EmployeeDto>();

                config.CreateMap<EmployeeListViewModel, Employee>();
                config.CreateMap<Employee, EmployeeListViewModel>();
            },
            AppDomain.CurrentDomain.GetAssemblies());
        
        builder.Services.AddAutoMapper(typeof(MappingProfile));
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

        app.UseNToastNotify();
        
        app.MapRazorPages();

        app.Run();
        #endregion
    }
}

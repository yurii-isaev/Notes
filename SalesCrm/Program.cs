using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.DataAccess;
using SalesCrm.DataAccess.Repositories;
using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Identities;
using SalesCrm.Services;
using SalesCrm.Services.Contracts;
using SalesCrm.Services.Contracts.Repositories;
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

        var connectionString = builder.Configuration
            .GetConnectionString("DefaultConnection") ?? throw
            new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var services = builder.Services;
        
        services.AddDbContext<AuthDbContext>(opts => opts.UseNpgsql(connectionString));
        services.AddDbContext<NewsDbContext>(opts => opts.UseNpgsql(connectionString));
        services.AddDbContext<EmployeeDbContext>(opts => opts.UseNpgsql(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddTransient<INewsService, NewsService>();
        services.AddTransient<INewsRepository, NewsRepository>();
        
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRepository, UserRepository>();
        
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        
        services.AddTransient<ITaxYearService, TaxYearService>();
        services.AddTransient<ITaxYearRepository, TaxYearRepository>();
        
        services.AddTransient<IPaymentRecordService, PaymentRecordService>();
        services.AddTransient<IPaymentRecordRepository, PaymentRecordRepository>();
        
        services.AddTransient<IRoleService, RoleService>();

        services
            .AddDefaultIdentity<User>(opts => opts.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

        services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
        {
            ProgressBar = true,
            PositionClass = ToastPositions.BottomRight
        });

        services.AddAutoMapper(config =>
            {
                config.CreateMap<EmployeeViewModel, Employee>();
                config.CreateMap<Employee, EmployeeViewModel>();
                config.CreateMap<EmployeeViewModel, EmployeeDto>();
                config.CreateMap<EmployeeDto, EmployeeViewModel>();

                config.CreateMap<EmployeeDto, Employee>();
                config.CreateMap<Employee, EmployeeDto>();

                config.CreateMap<EmployeeListViewModel, Employee>();
                config.CreateMap<Employee, EmployeeListViewModel>();
                
                config.CreateMap<TaxYearViewModel, TaxYearDto>();
                config.CreateMap<TaxYearDto, TaxYearViewModel>();
                config.CreateMap<TaxYear, TaxYearDto>();
                config.CreateMap<TaxYearDto, TaxYear>();
                
                config.CreateMap<PaymentRecordViewModel, PaymentRecordDto>();
                config.CreateMap<PaymentRecordDto, PaymentRecordViewModel>();
                config.CreateMap<PaymentRecord, PaymentRecordDto>();
                config.CreateMap<PaymentRecordDto, PaymentRecord>();
                
                config.CreateMap<UserViewModel, IdentityUser>();
                config.CreateMap<IdentityUser, UserViewModel>();
                
                config.CreateMap<NewsViewModel, NewsDto>();
                config.CreateMap<NewsDto, NewsViewModel>();
                config.CreateMap<News, NewsDto>();
                config.CreateMap<NewsDto, News>();
            },
            AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddAutoMapper(typeof(MappingProfile));
        
        services.AddRazorPages();
        services.AddMvc().AddRazorPagesOptions(opts =>
        {
            opts.Conventions.AddPageRoute("/Error", "/Error");
        });
        
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
        
        app.UseStatusCodePagesWithReExecute("/Error");

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=News}/{action=Index}/{id?}"
        );
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();

            endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("/News");
                return Task.CompletedTask;
            });
        });

        app.UseNToastNotify();

        app.Run();
        #endregion
    }
}

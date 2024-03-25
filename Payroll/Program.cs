using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Payroll.Controllers.Providers;
using Payroll.Controllers.ViewModels;
using Payroll.DataAccess;
using Payroll.DataAccess.Repositories;
using Payroll.Domains.Entities;
using Payroll.Domains.Identities;
using Payroll.Services;
using Payroll.Services.Contracts.Repositories;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Mapping;
using Payroll.Services.Objects;

namespace Payroll;

public class Program
{
  public static void Main(string[] args)
  {
    #region Web Application config

    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw
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

    services.AddTransient<IPaymentSlipService, PaymentSlipService>();
    services.AddTransient<IDeductionService, DeductionService>();

    services.AddTransient<InvoiceService>();

    services.AddTransient<IRoleService, RoleService>();
    services.AddTransient<IHttpStatusCodeDescriptionProvider, HttpStatusCodeDescriptionProvider>();

    services
      .AddDefaultIdentity<User>(opts => opts.SignIn.RequireConfirmedAccount = false)
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<AuthDbContext>();

    services
      .AddMvc()
      .AddRazorPagesOptions(opts => opts.Conventions.AddPageRoute("/Error", "/Error"))
      .AddNToastNotifyToastr(new ToastrOptions()
      {
        ProgressBar = true,
        PositionClass = ToastPositions.BottomRight
      });

    services.AddAutoMapper(config =>
      {
        // EmployeeViewModel -- EmployeeDto -- Employee
        config.CreateMap<EmployeeDto, EmployeeViewModel>().ReverseMap();
        config.CreateMap<EmployeeDto, Employee>().ReverseMap();

        // TaxYearViewModel -- TaxYearDto -- TaxYear
        config.CreateMap<TaxYearDto, TaxYearViewModel>().ReverseMap();
        config.CreateMap<TaxYearDto, TaxYear>().ReverseMap();

        // PaymentRecordViewModel -- PaymentRecordDto -- PaymentRecord
        config.CreateMap<PaymentRecordDto, PaymentRecordViewModel>().ReverseMap();
        config.CreateMap<PaymentRecordDto, PaymentRecord>().ReverseMap();

        // RoleViewModel -- RoleDto -- IdentityRole
        config.CreateMap<RoleDto, RoleViewModel>().ReverseMap();
        config.CreateMap<RoleDto, IdentityRole>().ReverseMap();

        // NewsViewModel -- NewsDto -- News
        config.CreateMap<NewsDto, NewsViewModel>().ReverseMap();
        config.CreateMap<NewsDto, News>().ReverseMap();

        // UserViewModel -- UserDto -- UserRole -- User
        config.CreateMap<UserDto, UserViewModel>().ReverseMap();
        config.CreateMap<UserDto, UserRole>().ReverseMap();
        config.CreateMap<UserRole, User>().ReverseMap();
      },
      AppDomain.CurrentDomain.GetAssemblies()
    );

    services.AddAutoMapper(typeof(MappingProfile));
    services.AddRazorPages();
    services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

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
        context.Response.Redirect("/news");
        return Task.CompletedTask;
      });
    });

    app.UseNToastNotify();

    app.Run();

    #endregion
  }
}

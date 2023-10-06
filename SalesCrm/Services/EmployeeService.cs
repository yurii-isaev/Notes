using AutoMapper;
using NToastNotify;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IToastNotification _toast;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public EmployeeService
    (
        IEmployeeRepository repo,
        ILogger<EmployeeService> logger,
        IToastNotification toast,
        IMapper mapper,
        IWebHostEnvironment environment
    )
    {
        _repository = repo;
        _logger = logger;
        _toast = toast;
        _mapper = mapper;
        _environment = environment;
    }

    public async Task<Employee> CreateEmployeeAsync(EmployeeDto dto)
    {
        try
        {
            var employee = _mapper.Map<Employee>(dto);

            employee.PaymentMethod = dto.PaymentMethod.ToString();

            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                var uploadDir = @"images/employee";
                var filename = Guid.NewGuid().ToString() + "-" + dto.ImageUrl.FileName;
                var path = Path.Combine(_environment.WebRootPath, uploadDir, filename);
                await dto.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                employee.ImageUrl = "/" + uploadDir + "/" + filename;
            }

            return await _repository.CreateEmployeeAsync(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("Employee service: " + ex.Message);
        }

        throw new InvalidOperationException();
    }

    public async Task<IEnumerable<Employee>> GetEmployeeListAsync()
    {
        try
        {
            return await _repository.GetEmployeeListAsync();
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Employee>();
        }
    }
}

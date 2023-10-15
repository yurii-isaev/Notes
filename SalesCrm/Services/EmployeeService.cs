using AutoMapper;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IWebHostEnvironment _environment;

    public EmployeeService
    (
        IEmployeeRepository repo,
        ILogger<EmployeeService> logger,
        IWebHostEnvironment environment
    )
    {
        _repository = repo;
        _logger = logger;
        _environment = environment;
    }

    public async Task<Employee> CreateEmployeeAsync(EmployeeDto dto)
    {
        try
        {
            var mapper = new MapperConfiguration(conf =>
            {
                conf.CreateMap<Employee, EmployeeDto>()
                    .ForMember(o => o.ImageUrl, opt => opt.MapFrom(e => e.ImageName));
            });

            var employee = mapper.CreateMapper().Map<EmployeeDto, Employee>(dto);

            employee.PaymentMethod = dto.PaymentMethod.ToString();

            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                var uploadDir = @"images/employees";
                var filename = dto.Name?.ToString() + "-" + dto.ImageUrl.FileName;
                var path = Path.Combine(_environment.WebRootPath, uploadDir, filename);
                await dto.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                employee.ImageName = "/" + uploadDir + "/" + filename;
            }

            return await _repository.CreateEmployeeAsync(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Exception create employee]: " + ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>> GetEmployeeListAsync()
    {
        try
        {
            return await _repository.GetEmployeeListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("[Get employee List]: " + ex.Message);
            return Enumerable.Empty<Employee>();
        }
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await _repository.GetEmployeeByIdAsync(employeeId);

        var mapper = new MapperConfiguration(conf =>
        {
            conf.CreateMap<Employee, EmployeeDto>()
                .ForMember(dto => dto.ImageUrl, opt => opt.Ignore());

            conf.CreateMap<EmployeeDto, Employee>();
        });

        var employeeDto = mapper
            .CreateMapper()
            .Map<Employee, EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task UpdateEmployeeAsync(EmployeeDto dto)
    {
        try
        {
            var mapper = new MapperConfiguration(conf =>
            {
                conf.CreateMap<Employee, EmployeeDto>()
                    .ForMember(o => o.ImageUrl, opt => opt.MapFrom(e => e.ImageName));

                conf.CreateMap<EmployeeDto, Employee>()
                    .ForMember(e => e.ImageName, opt => opt.MapFrom(o => o.ImageUrl!.FileName));
            });

            var employee = mapper.CreateMapper().Map<EmployeeDto, Employee>(dto);

            employee.PaymentMethod = dto.PaymentMethod.ToString();

            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                var uploadDir = @"images/employee";
                var filename = Guid.NewGuid().ToString() + "-" + dto.ImageUrl.FileName;
                var path = Path.Combine(_environment.WebRootPath, uploadDir, filename);
                await dto.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                employee.ImageName = "/" + uploadDir + "/" + filename;
            }

            await _repository.UpdateEmployeeAsync(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Error updating employee]: " + ex.Message);
            throw;
        }
    }

    public async Task DeleteEmployeeByIdAsync(Guid employeeId)
    {
        var dto = await GetEmployeeByIdAsync(employeeId);

        try
        {
            if (dto.ImageName != null && dto.ImageName.Length > 0)
            {
                var filename = dto.ImageName;
                await DeletePhotoFromStorage(filename);
            }

            await _repository.DeleteEmployeeAsync(employeeId);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Error deleting employee]: " + ex.Message);
            throw;
        }
    }

    private async Task DeletePhotoFromStorage(string filename)
    {
        var path = Path.Combine(_environment.WebRootPath, filename);
        var fullPath = $"wwwroot{path}";

        try
        {
            if (File.Exists(fullPath))
            {
                // File.Delete doesn't have an asynchronous version
                await Task.Run(() => File.Delete(fullPath));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("[Error deleting employee photo]: " + ex.Message);
        }
    }
}

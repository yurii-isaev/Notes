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
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public EmployeeService
    (
        IEmployeeRepository repo,
        ILogger<EmployeeService> logger,
        IMapper mapper,
        IWebHostEnvironment environment
    )
    {
        _repository = repo;
        _logger = logger;
        _mapper = mapper;
        _environment = environment;
    }

    public async Task<Employee> CreateEmployeeAsync(EmployeeDto dto)
    {
        try
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageName));
                cfg.CreateMap<EmployeeDto, Employee>().ForMember(dest => dest.ImageName,
                    opt => opt.MapFrom(src => src.ImageUrl!.FileName));
            });

            var employee = config.CreateMapper().Map<EmployeeDto, Employee>(dto);

            employee.PaymentMethod = dto.PaymentMethod.ToString();

            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                var uploadDir = @"images/employee";
                var filename = Guid.NewGuid().ToString() + "-" + dto.ImageUrl.FileName;
                var path = Path.Combine(_environment.WebRootPath, uploadDir, filename);
                await dto.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                employee.ImageName = "/" + uploadDir + "/" + filename;
            }

            return await _repository.CreateEmployeeAsync(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Create Employee]: " + ex.Message);
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
            _logger.LogError("[Get Employee List]: " + ex.Message);
            return Enumerable.Empty<Employee>();
        }
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await _repository.GetEmployeeByIdAsync(employeeId);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            cfg.CreateMap<EmployeeDto, Employee>();
        });

        var employeeDto = config.CreateMapper().Map<Employee, EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task UpdateEmployeeAsync(EmployeeDto dto)
    {
        try
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageName));
                cfg.CreateMap<EmployeeDto, Employee>().ForMember(dest => dest.ImageName,
                    opt => opt.MapFrom(src => src.ImageUrl!.FileName));
            });

            var employee = config.CreateMapper().Map<EmployeeDto, Employee>(dto);

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
            _logger.LogError("[Error updating Employee]: " + ex.Message);
            throw;
        }
    }
}

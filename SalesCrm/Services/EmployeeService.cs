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
    private readonly IToastNotification _toast;
    private readonly IWebHostEnvironment _environment;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository repo, IToastNotification toast, IWebHostEnvironment environment, IMapper mapper)
    {
        _repository = repo;
        _toast = toast;
        _environment = environment;
        _mapper = mapper;
    }

    public async Task<Employee> CreateEmployeeAsync(EmployeeInputDto dto)
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
            
            _toast.AddSuccessToastMessage("Employee successfully created");
            
            return await _repository.CreateEmployeeAsync(employee);
        }
        catch (Exception ex)
        {
            return null!;
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
            return Enumerable.Empty<Employee>();
        }
    }
}

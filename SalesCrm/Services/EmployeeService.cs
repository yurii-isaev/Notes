using AutoMapper;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Exceptions;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class EmployeeService : IEmployeeService
{
    readonly IEmployeeRepository _repository;
    readonly IMapper _mapper;
    readonly IWebHostEnvironment _environment;

    private const string UploadDir = @"images/employees/";

    public EmployeeService(IEmployeeRepository repository, IMapper mapper, IWebHostEnvironment environment)
    {
        _repository = repository;
        _mapper = mapper;
        _environment = environment;
    }

    public async Task CreateEmployeeAsync(EmployeeDto dto)
    {
        var mapper = new MapperConfiguration(conf =>
        {
            conf.CreateMap<Employee, EmployeeDto>()
                .ForMember(o => o.FormFile, opt => opt.MapFrom(e => e.ImageName));
        });

        var employee = mapper.CreateMapper().Map<EmployeeDto, Employee>(dto);

        employee.PaymentMethod = dto.PaymentMethod.ToString();

        if (dto.FormFile != null && dto.FormFile.Length > 0)
        {
            await AddEmployeePhoto(dto, employee);
        }

        if (employee.Name != null)
        {
            var nameExists = await _repository.EmployeeNameExistsAsync(employee.Name);

            if (!nameExists)
            {
                await _repository.CreateEmployeeAsync(employee);
            }
            else
            {
                throw new EmployeeExistsException("Employee already exists. The employee's name must be unique");
            }
        }
    }

    private async Task AddEmployeePhoto(EmployeeDto dto, Employee employee)
    {
        // Get the image file extension
        var fileExtension = Path.GetExtension(dto.FormFile!.FileName);

        // Form the file name using the name from the DTO and the file extension 
        var filename = $"{dto.Name}{fileExtension}";

        // Concatenates these path strings, taking into account the correct path separation for the operating system
        var path = Path.Combine(_environment.WebRootPath, UploadDir, filename);

        // Open the file for writing if it exists and create a new file if it doesn't exist
        await using var stream =
            new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

        // Performs an asynchronous copy of the contents of the file at the specified URL to a new file submitted by FileStream
        await dto.FormFile!.CopyToAsync(stream);

        // Assign a file name with a path to the corresponding property of the employee
        employee.ImageName = $"/{UploadDir}{filename}";
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeeListAsync(string keyword)
    {
        try
        {
            var employeeList = await _repository.GetEmployeeListAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                employeeList = employeeList.Where(emp => emp.Name!.Contains(keyword));
            }

            return _mapper.Map<IEnumerable<EmployeeDto>>(employeeList);
        }
        catch
        {
            return Enumerable.Empty<EmployeeDto>();
        }
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await _repository.GetEmployeeByIdAsync(employeeId);

        var mapper = new MapperConfiguration(conf =>
        {
            conf.CreateMap<Employee, EmployeeDto>().ForMember(dto => dto.FormFile, opt => opt.Ignore());
            conf.CreateMap<EmployeeDto, Employee>();
        });

        var employeeDto = mapper.CreateMapper().Map<Employee, EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task UpdateEmployeeAsync(EmployeeDto dto)
    {
        var mapper = new MapperConfiguration(conf =>
        {
            conf.CreateMap<Employee, EmployeeDto>()
                .ForMember(o => o.FormFile, opt => opt.MapFrom(e => e.ImageName));

            conf.CreateMap<EmployeeDto, Employee>()
                .ForMember(e => e.ImageName, opt => opt.MapFrom(o => o.FormFile!.FileName));
        });

        var employee = mapper.CreateMapper().Map<EmployeeDto, Employee>(dto);

        employee.PaymentMethod = dto.PaymentMethod.ToString();

        if (dto.FormFile != null && dto.FormFile.Length > 0)
        {
            var fileExtension = Path.GetExtension(dto.FormFile!.FileName);
            var filename = $"{dto.Name}{fileExtension}";
            var newEmployeePhotoPath = Path.Combine(_environment.WebRootPath, UploadDir, filename);
            var employeeObj = await _repository.GetEmployeeByIdAsync(dto.Id);
            var employeePhoto = employeeObj.ImageName;

            // Delete the old photo, if it exists
            if (!string.IsNullOrEmpty(employeePhoto))
            {
                var oldEmployeePhoto = Path.Combine(_environment.WebRootPath, employeePhoto.TrimStart('/'));

                if (File.Exists(oldEmployeePhoto))
                {
                    await Task.Run(() => File.Delete(oldEmployeePhoto));
                }
            }

            // This ensures that the file is properly closed after the image is copied, even if an exception occurs
            using (var fileStream = new FileStream(newEmployeePhotoPath, FileMode.Create))
            {
                await dto.FormFile!.CopyToAsync(fileStream);
            }

            employee.ImageName = $"/{UploadDir}{filename}";
        }

        await _repository.UpdateEmployeeAsync(employee);
    }


    public async Task DeleteEmployeeByIdAsync(Guid employeeId)
    {
        var dto = await GetEmployeeByIdAsync(employeeId);


        if (dto.ImageName != null && dto.ImageName.Length > 0)
        {
            var photoNamePath = dto.ImageName;
            await DeletePhotoFromStorage(photoNamePath);
        }

        await _repository.DeleteEmployeeAsync(employeeId);
    }

    public async Task<decimal> GetUnionFree(Guid id)
    {
        decimal unionFree = 0;
        var dto = await GetEmployeeByIdAsync(id);
        unionFree = (dto.UnionMemberStatus) ? 20m : 0;
        return unionFree;
    }

    private async Task DeletePhotoFromStorage(string photoNamePath)
    {
        var fullPath = $"wwwroot{photoNamePath}";

        if (File.Exists(fullPath))
        {
            // File.Delete doesn't have an asynchronous version
            await Task.Run(() => File.Delete(fullPath));
        }
    }
}

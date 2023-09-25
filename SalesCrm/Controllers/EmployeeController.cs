using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Domains.Entities;
using SalesCrm.Services;

namespace SalesCrm.Controllers;

public class EmployeeController : Controller
{
    private readonly EmployeeService _employeeService;
    private readonly IWebHostEnvironment _environment;
    private readonly IMapper _mapper;

    public EmployeeController(EmployeeService service, IWebHostEnvironment environment, IMapper mapper)
    {
        _employeeService = service;
        _environment = environment;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var vm = _employeeService.GetEmployeeListAsync();
        return View();
    }

    [Route("/employee/create")]
    [HttpGet]
    public Task<IActionResult> CreateEmployee()
    {
        var vm = new CreateEmployeeViewModel();
        return Task.FromResult<IActionResult>(View());
    }

    [Route("/employee/create")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var employee = _mapper.Map<Employee>(viewModel);

            if (viewModel.ImageUrl != null && viewModel.ImageUrl.Length > 0)
            {
                var uploadDir = @"images/employee";
                var filename = Guid.NewGuid().ToString() + "-" + viewModel.ImageUrl.FileName;
                var path = Path.Combine(_environment.WebRootPath, uploadDir, filename);
                await viewModel.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                employee.ImageUrl = "/" + uploadDir + "/" + filename;
            }

            await _employeeService.CreateEmployeeAsync(employee);
            
        }

        return RedirectToAction("Index");
    }
}

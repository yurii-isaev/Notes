using Microsoft.AspNetCore.Mvc;
using SalesCrm.Domains.Entities;
using SalesCrm.Services;
using SalesCrm.Views.ViewModels;

namespace SalesCrm.Controllers;

public class EmployeeController : Controller
{
    private readonly EmployeeService _employeeService;
    private readonly IWebHostEnvironment _environment;

    public EmployeeController(EmployeeService service, IWebHostEnvironment environment)
    {
        _employeeService = service;
        _environment = environment;
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
            var employee = new Employee
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Gender = viewModel.Gender,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                DateOfBirth = viewModel.DateOfBirth,
                DateJoined = viewModel.DateJoined,
                Insurance = viewModel.Insurance,
                PaymentMethod = viewModel.PaymentMethod,
                StudentLoanStatus = viewModel.StudentLoanStatus,
                UnionMemberStatus = viewModel.UnionMemberStatus,
                Address = viewModel.Address,
                City = viewModel.City,
                Postcode = viewModel.Postcode,
                Designation = viewModel.Designation
            };

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

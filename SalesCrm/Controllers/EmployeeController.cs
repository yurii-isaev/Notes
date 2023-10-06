using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Views;

namespace SalesCrm.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toast;

    public EmployeeController
    (
        IEmployeeService service,
        IMapper mapper,
        IToastNotification toast
    )
    {
        _employeeService = service;
        _mapper = mapper;
        _toast = toast;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        List<EmployeeListViewModel> list = _employeeService.GetEmployeeListAsync()
            .Result
            .Select(emp => _mapper.Map<EmployeeListViewModel>(emp))
            .ToList();

        var paginationList = PaginationList<EmployeeListViewModel>.Create(list, pageNumber, pageSize);

        return await Task.FromResult<IActionResult>(View(paginationList));
    }

    [Route("/employee/create")]
    [HttpGet]
    public async Task<IActionResult> CreateEmployee()
    {
        return await Task.FromResult<IActionResult>(View());
    }

    [Route("/employee/create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var dto = _mapper.Map<EmployeeDto>(viewModel);
                await _employeeService.CreateEmployeeAsync(dto);
                _toast.AddSuccessToastMessage("Employee successfully created");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating new employee");
                _toast.AddErrorToastMessage("Error creating new employee");
            }
        }

        return RedirectToAction("Index");
    }

    // [Route("/employee/edit/{id}")]
    // [HttpGet]
    // public async Task<IActionResult> EditEmployee(Guid employeeId)
    // {
    //     var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
    //     var viewModel = _mapper.Map<EmployeeViewModel>(employee);
    //
    //     return await Task.FromResult<IActionResult>(View(viewModel));
    // }
    //
    // [Route("/employee/edit/{id}")]
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(EmployeeViewModel viewModel)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         var employee = _mapper.Map<EmployeeInputDto>(viewModel);
    //
    //         if (viewModel.ImageUrl != null && viewModel.ImageUrl.Length > 0)
    //         {
    //             var uploadDir = @"images/employee";
    //             var filename = Guid.NewGuid().ToString() + "-" + viewModel.ImageUrl.FileName;
    //             var path = Path.Combine(_environment.WebRootPath, uploadDir, filename);
    //             await viewModel.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
    //             employee.ImageUrl = "/" + uploadDir + "/" + filename;
    //         }
    //
    //         await _employeeService.UpdateEmployeeAsync(employee);
    //     }
    //
    //     return RedirectToAction("Index");
    // }
}

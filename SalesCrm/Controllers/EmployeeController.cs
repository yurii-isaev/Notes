using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Views.Components.Pagination;

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
            catch (Exception)
            {
                ModelState.AddModelError("", "Error creating new employee");
                _toast.AddErrorToastMessage("Error creating new employee");
            }
        }

        return RedirectToAction("Index");
    }

    [Route("/employee/edit/{id}")]
    [HttpGet]
    public async Task<IActionResult> EditEmployee(Guid id)
    {
        var dto = await _employeeService.GetEmployeeByIdAsync(id);
        var viewModel = _mapper.Map<EmployeeViewModel>(dto);
        return await Task.FromResult<IActionResult>(View(viewModel));
    }

    [Route("/employee/edit/{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var dto = _mapper.Map<EmployeeDto>(viewModel);
                await _employeeService.UpdateEmployeeAsync(dto);
                _toast.AddSuccessToastMessage("Employee successfully updated");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error updated employee");
                _toast.AddErrorToastMessage("Error updated employee");
            }
        }

        return RedirectToAction("Index");
    }

    [Route("/employee/delete/{id}")]
    [HttpGet]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _employeeService.DeleteEmployeeByIdAsync(id);
                _toast.AddSuccessToastMessage("Employee successfully deleted");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error deleted employee");
                _toast.AddErrorToastMessage("Error deleted employee");
            }
        }

        return RedirectToAction("Index");
    }
}

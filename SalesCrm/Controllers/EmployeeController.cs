using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.Options;
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

    public EmployeeController(IEmployeeService service, IMapper mapper, IToastNotification toast)
    {
        _employeeService = service;
        _mapper = mapper;
        _toast = toast;
    }
    
    [HttpGet]
    [Route("/employee")]
    public async Task<IActionResult> Index(SearchOptions? searcher, PaginationOptions pagination)
    {
        try
        {
            string keyword = searcher!.Keyword!;

            var employeeList = _employeeService.GetEmployeeListAsync(keyword)
                .Result
                .Select(emp => _mapper.Map<EmployeeViewModel>(emp))
                .ToList();

            var orderedEmployeeList = employeeList
                .OrderByDescending(s => s.DateJoined)
                .ToList();
            
            var paginationList = PaginationList<EmployeeViewModel>
                .Create(orderedEmployeeList, pagination.PageNumber, pagination.PageSize);

            return await Task.FromResult<IActionResult>(View(paginationList));
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
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
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

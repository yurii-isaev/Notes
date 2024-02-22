using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Payroll.Controllers.Options;
using Payroll.Controllers.ViewModels;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Exceptions;
using Payroll.Services.Objects;
using Payroll.Utils.Reports;
using Payroll.Views.Shared.Pagination;

namespace Payroll.Controllers;

[Authorize(Roles = "Admin, Manager")]
public class EmployeeController : BaseController
{
    readonly IEmployeeService _employeeService;
    readonly IMapper _mapper;
    readonly IToastNotification _toast;

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
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return RedirectToAction(nameof(Error));
        }
    }

    [HttpGet]
    [Route("/employee/create")]
    public async Task<IActionResult> CreateEmployee()
    {
        return await Task.FromResult<IActionResult>(View());
    }

    [HttpPost]
    [Route("/employee/create")]
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
            catch (EmployeeExistsException ex)
            {
                _toast.AddErrorToastMessage("Error creating new employee");
                ModelState.AddModelError("", ex.Message);
                return View("CreateEmployee");
            }
        }
        else
        {
            return RedirectToAction(nameof(Error), new
            {
                statusCode = 400,
                message = "Bad Request"
            });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("/employee/edit/{id}")]
    public async Task<IActionResult> EditEmployee(Guid id)
    {
        var dto = await _employeeService.GetEmployeeByIdAsync(id);
        var viewModel = _mapper.Map<EmployeeViewModel>(dto);
        return await Task.FromResult<IActionResult>(View(viewModel));
    }

    [HttpPost]
    [Route("/employee/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var dto = _mapper.Map<EmployeeDto>(viewModel);
                
                // Checking if a new image has been uploaded
                if (dto.FormFile == null)
                {
                    // If not, create a new FormFile using Model.ImageName
                    dto.FormFile = new FormFile(new MemoryStream(), 0, 0, "FormFile", viewModel.ImageName!);
                }
                
                await _employeeService.UpdateEmployeeAsync(dto);
                _toast.AddSuccessToastMessage("Employee successfully updated");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                _toast.AddErrorToastMessage("Error updated employee");
                return RedirectToAction(nameof(Error));
            }
        }
        else
        {
            return RedirectToAction(nameof(Error), new
            {
                statusCode = 400,
                message = "Bad Request"
            });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("/employee/delete/{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _employeeService.DeleteEmployeeByIdAsync(id);
                _toast.AddSuccessToastMessage("Employee successfully deleted");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                _toast.AddErrorToastMessage("Error deleted employee");
                return RedirectToAction(nameof(Error));
            }
        }

        return RedirectToAction(nameof(Index));
    }
}

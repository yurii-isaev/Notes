using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Payroll.Controllers.Providers;
using Payroll.Controllers.ViewModels;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Exceptions;
using Payroll.Services.Objects;
using Payroll.Utils.Reports;

namespace Payroll.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController : BaseController
{
  readonly IRoleService _roleService;
  readonly IMapper _mapper;
  readonly IToastNotification _toast;
  readonly IHttpStatusCodeDescriptionProvider _httpStatusProvider;

  public RolesController
  (
    IRoleService roleService,
    IMapper mapper,
    IToastNotification toast,
    IHttpStatusCodeDescriptionProvider httpStatusProvider
  )
  {
    _roleService = roleService;
    _mapper = mapper;
    _toast = toast;
    _httpStatusProvider = httpStatusProvider;
  }

  [HttpGet]
  [Route("/roles")]
  public async Task<IActionResult> Index()
  {
    try
    {
      var roleList = _roleService.GetRolesAsync()
        .Result
        .Select(dto => _mapper.Map<RoleViewModel>(dto))
        .ToList();

      return await Task.FromResult<IActionResult>(View(roleList));
    }
    catch (Exception ex)
    {
      Logger.LogError(ex);
      return RedirectToAction(nameof(Error));
    }
  }

  [HttpGet]
  [Route("/roles/create")]
  public Task<IActionResult> CreateRole()
  {
    return Task.FromResult<IActionResult>(View());
  }

  [HttpPost]
  [Route("/roles/create")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(RoleViewModel viewModel)
  {
    if (ModelState.IsValid)
    {
      try
      {
        var dto = _mapper.Map<RoleDto>(viewModel);
        await _roleService.CreateRoleAsync(dto);
        _toast.AddSuccessToastMessage("Role create successfully");
      }
      catch (RoleExistsException ex)
      {
        _toast.AddErrorToastMessage("Error creating role");
        ModelState.AddModelError("", ex.Message);
        return View("CreateRole");
      }
      catch (HttpRequestException ex)
      {
        _toast.AddErrorToastMessage("Error created role");
        int? statusCode = (int?) ex.StatusCode;

        if (statusCode.HasValue)
        {
          string statusDescription = _httpStatusProvider.GetStatusDescription(statusCode.Value)!;

          return RedirectToAction("Error", new
          {
            statusCode = statusCode.Value,
            message = statusDescription
          });
        }
        else
        {
          return RedirectToAction("Error");
        }
      }
    }

    return RedirectToAction("Index");
  }

  [HttpGet]
  [Route("/roles/edit/{id}")]
  public async Task<IActionResult> EditRole(string id)
  {
    if (ModelState.IsValid)
    {
      try
      {
        var role = await _roleService.GetRoleNameAsync(id);
        var viewModel = _mapper.Map<RoleViewModel>(role);
        return await Task.FromResult<IActionResult>(View(viewModel));
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
        return RedirectToAction("Error");
      }
    }

    return RedirectToAction("Index");
  }

  [HttpPost]
  [Route("/roles/edit/{id}")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(RoleViewModel viewModel)
  {
    if (ModelState.IsValid)
    {
      try
      {
        var dto = _mapper.Map<RoleDto>(viewModel);
        await _roleService.UpdateRoleAsync(dto);
        _toast.AddSuccessToastMessage("Role updated successfully");
      }
      catch (RoleExistsException ex)
      {
        _toast.AddErrorToastMessage("Error updating role");
        ModelState.AddModelError("", ex.Message);
        return View("EditRole");
      }
      catch (HttpRequestException ex)
      {
        _toast.AddErrorToastMessage("Error updating role");
        int? statusCode = (int?) ex.StatusCode;

        if (statusCode.HasValue)
        {
          string statusDescription = _httpStatusProvider.GetStatusDescription(statusCode.Value)!;

          return RedirectToAction("Error", new
          {
            statusCode = statusCode.Value,
            message = statusDescription
          });
        }
        else
        {
          return RedirectToAction("Error");
        }
      }
    }

    return RedirectToAction("Index");
  }

  [HttpGet]
  [Route("/roles/delete/{id}")]
  public async Task<IActionResult> DeleteRole(string id)
  {
    try
    {
      _toast.AddSuccessToastMessage("Role delete successfully");
      await _roleService.DeleteRoleAsync(id);
    }
    catch (Exception ex)
    {
      Logger.LogError(ex);
      _toast.AddErrorToastMessage("Error deleting role");
      return RedirectToAction("Error");
    }

    return RedirectToAction("Index");
  }
}

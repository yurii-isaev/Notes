using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.Providers;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Exceptions;
using SalesCrm.Services.Input;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toast;
    private readonly IHttpStatusCodeDescriptionProvider _httpStatusProvider;

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
        catch (Exception)
        {
            return RedirectToAction("Error");
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
                    // Handle the case where StatusCode is null.
                    // For example, you might want to use a default status code.
                    return RedirectToAction("Error", new
                    {
                        message = 500
                    });
                }
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/roles/edit/{id}")]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await _roleService.GetRoleNameAsync(id);
        return View(role);
    }

    [HttpPost]
    [Route("/roles/edit/{id}")]
    public async Task<IActionResult> Edit(RoleViewModel role)
    {
        await _roleService.UpdateRoleAsync(role);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/roles/delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        await _roleService.DeleteRoleAsync(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode, string? message)
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = statusCode ?? 500,
            Message = message
        });
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Payroll.Controllers.Providers;
using Payroll.Controllers.ViewModels;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;
using Payroll.Utils.Reports;

namespace Payroll.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : BaseController
{
    readonly IMapper _mapper;
    readonly IUserService _userService;
    readonly IHttpStatusCodeDescriptionProvider _httpStatusProvider;
    readonly IToastNotification _toast;

    public UsersController
    (
        IUserService userService,
        IMapper mapper,
        IHttpStatusCodeDescriptionProvider httpStatusProvider,
        IToastNotification toast
    )
    {
        _userService = userService;
        _mapper = mapper;
        _httpStatusProvider = httpStatusProvider;
        _toast = toast;
    }

    [HttpGet]
    [Route("/users")]
    public async Task<IActionResult> Index()
    {
        try
        {
            List<UserViewModel> list = _userService.GetUserListAsync()
                .Result
                .Select(user => _mapper.Map<UserViewModel>(user))
                .ToList();

            return await Task.FromResult<IActionResult>(View(list));
        }
        catch (HttpRequestException ex)
        {
            int? statusCode = (int?) ex.StatusCode;

            if (statusCode.HasValue)
            {
                string statusDescription = _httpStatusProvider.GetStatusDescription(statusCode.Value)!;
                return RedirectToAction("Error", new {statusCode.Value, statusDescription});
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return RedirectToAction(nameof(Error));
        }
    }

    [HttpGet]
    [Route("/user/edit/{id}")]
    public async Task<IActionResult> EditUser(string id)
    {
        try
        {
            var dto = await _userService.GetUserByIdAsync(id);
            var viewModel = _mapper.Map<UserViewModel>(dto);

            string date = viewModel.Created.ToString("dd.MM.yyyy HH:mm");
            viewModel.Created = Convert.ToDateTime(date);

            return View(viewModel);
        }
        catch (HttpRequestException ex)
        {
            int? statusCode = (int?) ex.StatusCode;

            if (statusCode.HasValue)
            {
                string statusDescription = _httpStatusProvider.GetStatusDescription(statusCode.Value)!;
                return RedirectToAction("Error", new {statusCode.Value, statusDescription});
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }
        catch
        {
            return RedirectToAction("Error", new {statusCode = 400, message = "Bad Request"});
        }
    }

    [HttpPost]
    [Route("/user/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserViewModel viewModel)
    {
        try
        {
            var dto = _mapper.Map<UserDto>(viewModel);
            await _userService.UpdateUserAsync(dto);
            _toast.AddSuccessToastMessage("User successfully updated");
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            int? statusCode = (int?) ex.StatusCode;

            if (statusCode.HasValue)
            {
                string statusDescription = _httpStatusProvider.GetStatusDescription(statusCode.Value)!;
                return RedirectToAction("Error", new {statusCode.Value, statusDescription});
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return RedirectToAction(nameof(Error));
        }
    }

    [Route("/users/block/{id}")]
    public async Task<IActionResult> BlockUser(string id)
    {
        try
        {
            await _userService.BlockUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return RedirectToAction(nameof(Error));
        }
    }

    [Route("/users/unblock/{id}")]
    public async Task<IActionResult> UnBlockUser(string id)
    {
        try
        {
            await _userService.UnBlockUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return RedirectToAction(nameof(Error));
        }
    }
}

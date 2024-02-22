using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Payroll.Controllers.ViewModels;

namespace Payroll.Controllers;

public abstract class BaseController : Controller
{
    [
        ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true
        )
    ]
    public IActionResult Error(int? statusCode, string? message)
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = statusCode ?? 500,
            Message = message ?? "Internal Server Error"
        });
    }
}

using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Payroll.Controllers.ViewModels;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;
using Payroll.Utils.Reports;

namespace Payroll.Pages.News;

[Authorize(Roles = "Admin, Manager")]
public class CreateModel : PageModel
{
  readonly INewsService _newsService;
  readonly IMapper _mapper;
  readonly IToastNotification _toast;

  public CreateModel(INewsService service, IMapper mapper, IToastNotification toast)
  {
    _newsService = service;
    _mapper = mapper;
    _toast = toast;
  }

  [BindProperty(Name = "newsId")] public Guid NewsId { get; set; }

  [BindProperty] public NewsViewModel News { get; set; } = null!;


  public async Task<IActionResult> OnPostAsync()
  {
    // Registation only
    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (ModelState.IsValid)
    {
      try
      {
        if (!string.IsNullOrEmpty(currentUserId))
        {
          News.AuthorId = currentUserId;
          var dto = _mapper.Map<NewsDto>(News);
          await _newsService.CreateNewsAsync(dto);

          _toast.AddSuccessToastMessage("News created successfully");
        }
        else
        {
          return Page();
        }
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("", "Error created news");
        _toast.AddErrorToastMessage("Error created news");
        Logger.LogError(ex);

        return RedirectToPage("Error", new {errorMessage = ex.Message});
      }
    }

    return RedirectToPage("/news");
  }
}

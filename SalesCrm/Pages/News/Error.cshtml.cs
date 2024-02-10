using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SalesCrm.Pages.News;

[
    ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true),
    IgnoreAntiforgeryToken
]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
    
    public ActionResult OnPost()
    {
        HandleError();
        return Page();
    }
        
    private void HandleError()
    {
        HttpContext.Features.Get<IExceptionHandlerPathFeature>();
    }
}
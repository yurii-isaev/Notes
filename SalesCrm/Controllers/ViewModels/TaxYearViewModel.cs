using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Controllers.ViewModels;

public class TaxYearViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Year Of Tax is required")]
    public string? YearOfTax { get; set; }
}

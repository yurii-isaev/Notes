using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.Validation;

namespace SalesCrm.Controllers.ViewModels;

public class TaxYearViewModel
{
    [HiddenInput(DisplayValue = false)]
    public Guid Id { get; set; }

    [
        DisplayName("Year Of Tax"),
        Required,
        StringLength(4, MinimumLength = 4, ErrorMessage = "Invalid input year."),
        ValidateDate
    ]
    public string? YearOfTax { get; set; }
}

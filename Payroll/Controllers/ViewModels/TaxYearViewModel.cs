using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Payroll.Controllers.Validation;

namespace Payroll.Controllers.ViewModels;

public class TaxYearViewModel
{
    [HiddenInput(DisplayValue = false)]
    public Guid Id { get; set; }

    [
        DisplayName("Year Of Tax"),
        ValidateDate
    ]
    public string? YearOfTax { get; set; }
}

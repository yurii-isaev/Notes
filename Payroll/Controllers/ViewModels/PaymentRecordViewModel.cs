using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Payroll.Domains.Entities;

namespace Payroll.Controllers.ViewModels;

public class PaymentRecordViewModel
{
    [HiddenInput(DisplayValue = false)]
    public Guid Id { get; set; }

    [
        Display(Name = "Employee"),
        Required
    ]
    public Guid EmployeeId { get; set; }

    public Employee? Employee { get; set; }

    public string? Name { get; set; }

    public string? InsuranceNumber { get; set; }

    [Display(Name = "Pay Date")]
    public DateTime PayDate { get; set; } = DateTime.UtcNow;

    [
        Display(Name = "Pay Month"),
        Required
    ]
    public string? PayMonth { get; set; } = DateTime.Today.Month.ToString();

    [
        Display(Name = "Tax Year"),
        Required
    ]
    public Guid TaxYearId { get; set; }

    public TaxYear? TaxYear { get; set; }

    public string? TaxCode { get; set; } = "103";

    [
        Display(Name = "Hourly Rate"),
        Required
    ]
    public decimal HourlyRate { get; set; }

    [
        Display(Name = "Hours Worked"),
        Required
    ]
    public decimal HoursWorked { get; set; }

    public decimal ContractualHours { get; set; } = 35;

    public decimal OvertimeHours { get; set; }

    public decimal ContractualEarnings { get; set; }

    public decimal OvertimeEarnings { get; set; }

    public decimal Tax { get; set; }

    public decimal UnionFree { get; set; }

    [Display(Name = "Total Earnings")]
    public decimal TotalEarnings { get; set; }

    [Display(Name = "Total Deduction")]
    public decimal TotalDeduction { get; set; }

    [Display(Name = "Net Payment")]
    public decimal NetPayment { get; set; }
}

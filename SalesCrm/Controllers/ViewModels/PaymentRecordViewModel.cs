using System.ComponentModel.DataAnnotations;
using SalesCrm.Domains.Entities;

namespace SalesCrm.Controllers.ViewModels;

public class PaymentRecordViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Employee")]
    public Guid EmployeeId { get; set; }

    public Employee? Employee { get; set; }

    public string? Name { get; set; }
    
    public string? InsuranceNumber { get; set; }

    public DateTime PayDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Pay Month")]
    public string? PayMonth { get; set; } = DateTime.Today.Month.ToString();

    [Display(Name = "Tax Year")]
    public Guid TaxYearId { get; set; }

    public TaxYear? TaxYear { get; set; }

    [
        Display(Name = "Hourly Rate"),
        Required(ErrorMessage = "The field Hourly Rate is required.")
    ]
    public decimal HourlyRate { get; set; }

    [
        Display(Name = "Hours Worked"),
        Required(ErrorMessage = "The field Hours Worked is required.")
    ]
    public decimal HoursWorked { get; set; }

    public decimal ContractualHours { get; set; } = 150;

    public decimal OvertimeHours { get; set; }

    public decimal ContractualEarnings { get; set; }

    public decimal OvertimeEarnings { get; set; }

    public decimal Tax { get; set; }

    public decimal UnionFree { get; set; }

    public decimal TotalEarnings { get; set; }

    public decimal TotalDeduction { get; set; }

    public decimal NetPayment { get; set; }
}
